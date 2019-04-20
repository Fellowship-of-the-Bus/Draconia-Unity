using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControl : MonoBehaviour {
  // Camera used by the player
  private Camera PlayerCam;
  // GameObject responsible for the management of the game
  private GameManager gameManager;
  private Channel channel = new Channel("PlayerControl", false);

  [HideInInspector]
  public bool preGame = true;

  [HideInInspector]
  public Tile currentHoveredTile = null;

  // Use this for initialization
  void Start() {
    PlayerCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    gameManager = GameManager.get;
  }

  // Update is called once per frame
  void Update() {
    // Don't allow input on AI turn
    if (gameManager.UILocked()) return;

    if (Input.GetKeyDown(KeyCode.Return)) {
      gameManager.endTurnWrapper();
    }

    GetMouseInputs();
  }

  // Detect Mouse Inputs
  void GetMouseInputs() {


    handleHovered(gameManager.getHovered(PlayerCam));
    handleClicked(gameManager.getClicked(PlayerCam));
  }

  bool overUI() {
    //Decides whether the pointer is over a ui element or gameobject.
    //Despite what the name implies, it returns true if the pointer is over a ui element
    //and false if it is over a game object.
    bool ret = EventSystem.current.IsPointerOverGameObject();
    if (ret) {
      channel.Log("Over UI");
    }
    return ret;
  }

  void handleHovered(GameObject hoveredObject) {
    channel.Log("handleHovered: {0}", hoveredObject);
    if (gameManager.SelectedPiece) gameManager.lineTo(gameManager.SelectedPiece.gameObject);
    if (overUI() || hoveredObject == null) return;
    if (hoveredObject.transform.parent == null) return;
    Map map = gameManager.map;
    BattleCharacter selectedCharacter = null;
    if (!preGame) selectedCharacter = gameManager.SelectedPiece.GetComponent<BattleCharacter>();

    ActiveSkill s = null;
    if (gameManager.SelectedSkill != -1 && !preGame) s = selectedCharacter.equippedSkills[gameManager.SelectedSkill];


    //handle multicubes
    if (hoveredObject.transform.parent.CompareTag("Cube")) {
      hoveredObject = hoveredObject.transform.parent.gameObject;
    }
    Transform parent = hoveredObject.transform.parent;
    while (parent != null && !parent.CompareTag("Unit")) {
      parent = parent.parent;
    }
    if (parent) hoveredObject = parent.gameObject;
    bool isTile = hoveredObject.CompareTag("Cube");
    bool isPiece = hoveredObject.CompareTag("Unit");
    Tile hoveredTile = map.getTile(hoveredObject.transform.position);
    if (hoveredTile) currentHoveredTile = hoveredTile;

    channel.Log("Hovering over tile? {0}; piece? {1}", isTile, isPiece);

    if (preGame){
      map.clearColour();
      GameSceneController.get.resetStartTileColour();
      if (!isTile && !isPiece) return;
      if (hoveredTile != null) {
        hoveredTile.setColor(Color.blue);
      }
    } else {
      gameManager.selectTarget(hoveredObject);
      //if pieces are moving around, skip
      if (gameManager.moving || (!isTile && !isPiece)) return;

      //handle movement based tile colouring
      if (isTile) {
        if (gameManager.gameState == GameState.moving || (s != null && s is Sprint)) {
          int rangeToMove = gameManager.moveRange;
          if (s is Sprint) rangeToMove += s.range;

          if (hoveredTile.distance <= rangeToMove && !hoveredTile.occupied()) {
            map.setPath(hoveredTile.position);
          } else {
            map.clearPath();
          }
          map.setTileColours();
        }
      }
      //handle attack based tile colouring:
      if ((gameManager.gameState == GameState.attacking && s != null)) {
        //handle attack
        map.setTileColours(hoveredTile);
        if (isPiece && s.useLos) gameManager.lineTo(hoveredObject);
      }

      gameManager.tInfo.setTile(hoveredTile);
    }
  }

  void handleClicked(GameObject clickedObject) {
    if (overUI() || clickedObject == null || gameManager.moving) return;
    //handle multicubes
    if (clickedObject.transform.parent.CompareTag("Cube")) {
      clickedObject = clickedObject.transform.parent.gameObject;
    } else if (clickedObject.transform.parent.CompareTag("Unit")) {
      clickedObject = clickedObject.transform.parent.gameObject;
    }

    bool isTile = clickedObject.CompareTag("Cube");
    bool isPiece = clickedObject.CompareTag("Unit");
    Tile clickedTile = gameManager.map.getTile(clickedObject.transform.position);

    if (preGame) {
      BattleCharacter clickedChar = clickedObject.GetComponent<BattleCharacter>();

      if (clickedChar != null && clickedChar.team == 0 && clickedChar.aiType == AIType.None) {
        if (clickedChar == selectedCharacter) {
          deselect();
          return;
        } else if (selectedCharacter == null) {
          selChar(clickedChar);
          return;
        } else {
          swap(clickedChar);
          return;
        }
      } else if (isTile && clickedTile != null && selectedCharacter != null) {
        //if occupied swap
        if (clickedTile.occupied()) {
          swap(clickedTile.occupant);
        } else {
          place(clickedTile);
        }
        return;
      }
    } else {
      if (!isTile && !isPiece) return;

      //handle clicking on tile (move or attack ground)
      if (isTile) {
        if (gameManager.gameState == GameState.moving) {
          gameManager.movePiece(clickedObject.transform.position);
        } else if (gameManager.gameState == GameState.attacking && gameManager.SelectedSkill >= 0) {
          gameManager.attackTarget(clickedTile);
        }
      } else if (gameManager.gameState == GameState.attacking && gameManager.SelectedSkill >= 0) {
        gameManager.attackTarget(clickedTile);
      }
    }
  }

  [HideInInspector]
  public BattleCharacter selectedCharacter = null;
  void deselect() {
    selectedCharacter = null;
  }
  void selChar(BattleCharacter c) {
    selectedCharacter = c;
  }

  //swaps locations of clicked and selected chars
  void swap(BattleCharacter clickedChar) {
    //precondition: selectedCharacter != null
    Debug.AssertFormat(selectedCharacter != null, "swap called with null selectedCharacter");

    //try to swap same characters return;
    if (clickedChar == selectedCharacter) {
      deselect();
      return;
    }

    Tile selTile = selectedCharacter.curTile;
    Tile clickTile = clickedChar.curTile;

    //swap occupants
    selTile.occupant = clickedChar;
    clickTile.occupant = selectedCharacter;

    //set curTiles
    clickedChar.curTile = selTile;
    selectedCharacter.curTile = clickTile;

    //set positions
    selectedCharacter.gameObject.transform.position = selectedCharacter.curTile.position;
    clickedChar.gameObject.transform.position = clickedChar.curTile.position;

    //deselected character
    deselect();
  }
  //places selected char at tile
  void place(Tile t) {
    //precondition: t is unoccupied
    Debug.AssertFormat(!t.occupied(), "tried to place on occupied tile");
    //check it is a valid start tile
    if (!GameSceneController.get.validStartTile(t)) return;
    //break original tile links
    selectedCharacter.curTile.occupant = null;
    selectedCharacter.curTile = null;

    //set new links
    selectedCharacter.curTile = t;
    t.occupant = selectedCharacter;

    selectedCharacter.gameObject.transform.position = t.position;

    //deselected the character
    deselect();
  }
}

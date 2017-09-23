using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControl : MonoBehaviour {
  private Camera PlayerCam;
  // Camera used by the player
  private GameManager gameManager;
  // GameObject responsible for the management of the game

  public bool preview = true;

  // Use this for initialization
  void Start() {
    PlayerCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    gameManager = gameObject.GetComponent<GameManager>();
  }

  // Update is called once per frame
  void Update() {
    GetMouseInputs();
  }

  // Detect Mouse Inputs
  void GetMouseInputs() {
    // Don't allow input on AI turn
    if (gameManager.UILocked()) return;

    handleHovered(gameManager.getHovered(PlayerCam));
    handleClicked(gameManager.getClicked(PlayerCam));
  }

  void handleHovered(GameObject hoveredObject) {
    gameManager.lineTo(gameManager.SelectedPiece);
    if (hoveredObject == null) return;
    Map map = gameManager.map;
    BattleCharacter selectedCharacter = null;
    if (!preview) selectedCharacter = gameManager.SelectedPiece.GetComponent<BattleCharacter>();

    ActiveSkill s = null;
    if (gameManager.SelectedSkill != -1 && !preview) s = selectedCharacter.equippedSkills[gameManager.SelectedSkill];

    //handle multicubes
    if (hoveredObject.transform.parent.tag == "Cube") {
      hoveredObject = hoveredObject.transform.parent.gameObject;
    }
    Transform parent = hoveredObject.transform.parent;
    while (parent != null && parent.tag != "Unit") {
      parent = parent.parent;
    }
    if (parent) hoveredObject = parent.gameObject;
    bool isTile = hoveredObject.tag == "Cube";
    bool isPiece = hoveredObject.tag == "Unit";
    Tile hoveredTile = map.getTile(hoveredObject.transform.position);
    BattleCharacter hoveredPiece = hoveredObject.GetComponent<BattleCharacter>();
    gameManager.selectTarget(hoveredPiece);


    if (preview){
      map.clearColour();
      GameSceneController.get.resetStartTileColour();
      if (!isTile && !isPiece) return;
      if (hoveredTile != null) {
        hoveredTile.setColor(Color.blue);
      }
    } else {
      //if pieces are moving around, skip
      if (gameManager.moving || (!isTile && !isPiece)) return;

      //handle movement based tile colouring
      if (isTile) {
        if (gameManager.gameState == GameState.moving || (s != null && s is Sprint)) {
          int rangeToMove = gameManager.moveRange;
          if (s is Sprint) rangeToMove += s.range;

          if (hoveredTile.distance <= rangeToMove) {
            map.setPath(hoveredTile.position);
          } else {
            map.clearPath();
          }
          map.setTileColours();
        }
      }
      //handle attack based tile colouring:
      if ((gameManager.gameState == GameState.attacking && s != null && s.getTargets().Contains(hoveredTile))) {
        //handle attack
        map.setTileColours(hoveredTile);
        if (isPiece) gameManager.lineTo(hoveredObject);
      }

      gameManager.tInfo.setTile(hoveredTile);
    }
  }

  void handleClicked(GameObject clickedObject) {
    if (EventSystem.current.IsPointerOverGameObject() || clickedObject == null || gameManager.moving) return;
    //handle multicubes
    if (clickedObject.transform.parent.tag == "Cube") {
      clickedObject = clickedObject.transform.parent.gameObject;
    }

    bool isTile = clickedObject.tag == "Cube";
    bool isPiece = clickedObject.tag == "Unit";
    Tile clickedTile = gameManager.map.getTile(clickedObject.transform.position);

    if (preview) {
      BattleCharacter clickedChar = clickedObject.GetComponent<BattleCharacter>();

      if (clickedChar != null && clickedChar.team == 0) {
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

  BattleCharacter selectedCharacter = null;
  //TODO: update colouring of selected characters .....
  void deselect() {
    //selectedCharacter.GetComponent<Renderer>().material.color = Color.white;
    selectedCharacter = null;
  }
  void selChar(BattleCharacter c) {
    selectedCharacter = c;
    //selectedCharacter.GetComponent<Renderer>().material.color = Color.red;
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

using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControl : MonoBehaviour {
  private Camera PlayerCam;
  // Camera used by the player
  private GameManager gameManager;
  // GameObject responsible for the management of the game

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
    BattleCharacter selectedCharacter =  gameManager.SelectedPiece.GetComponent<BattleCharacter>();

    ActiveSkill s = null;
    if (gameManager.SelectedSkill != -1) s = selectedCharacter.equippedSkills[gameManager.SelectedSkill];

    //handle multicubes
    if (hoveredObject.transform.parent.tag == "Cube") {
      hoveredObject = hoveredObject.transform.parent.gameObject;
    }

    bool isTile = hoveredObject.tag == "Cube";
    bool isPiece = hoveredObject.tag == "Unit";
    Tile hoveredTile = map.getTile(hoveredObject.transform.position);
    BattleCharacter hoveredPiece = hoveredObject.GetComponent<BattleCharacter>();
    gameManager.selectTarget(hoveredPiece);

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

  void handleClicked(GameObject clickedObject) {
    if (EventSystem.current.IsPointerOverGameObject() || clickedObject == null || gameManager.moving) return;
    //handle multicubes
    if (clickedObject.transform.parent.tag == "Cube") {
      clickedObject = clickedObject.transform.parent.gameObject;
    }

    bool isTile = clickedObject.tag == "Cube";
    bool isPiece = clickedObject.tag == "Unit";
    Tile clickedTile = gameManager.map.getTile(clickedObject.transform.position);

    if (!isTile && !isPiece) return;

    //handle clicking on tile (move or attack ground)
    if (isTile) {
      if (gameManager.gameState == GameState.moving) {
        gameManager.waitToEndTurn(gameManager.movePiece(clickedObject.transform.position));
      } else if (gameManager.gameState == GameState.attacking && gameManager.SelectedSkill >= 0) {
        gameManager.attackTarget(clickedTile);
      }
    } else if (gameManager.gameState == GameState.attacking && gameManager.SelectedSkill >= 0) {
      gameManager.attackTarget(clickedTile);
    }
  }
}

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
    if (gameManager.UILocked()) {
      Map map = gameManager.map;
      GameObject clickedObject = gameManager.getClicked(PlayerCam);
      GameObject hoveredObject = gameManager.getHovered(PlayerCam);
      BattleCharacter selectedCharacter =  gameManager.SelectedPiece.GetComponent<BattleCharacter>();
      Skill s = null;
      if (gameManager.SelectedSkill != -1) s = selectedCharacter.equippedSkills[gameManager.SelectedSkill];

      if (hoveredObject && (hoveredObject.tag == "Cube" || hoveredObject.transform.parent.tag == "Cube") && !gameManager.moving) {
        // In the case of a multisided cube the parent is the true hovered object
        if (hoveredObject.transform.parent.tag == "Cube") {
          hoveredObject = hoveredObject.transform.parent.gameObject;
        }

        if (gameManager.gameState == GameState.moving || (s != null && s is Sprint)) {
          Vector3 coord = new Vector3(hoveredObject.transform.position.x, hoveredObject.transform.position.y + 1, hoveredObject.transform.position.z);
          Tile t = map.getTile(coord);
          int rangeToMove = gameManager.moveRange;
          if (s is Sprint) {
            rangeToMove += s.range;
          }
          if (t.distance <= rangeToMove) {
            map.setPath(coord);
          } else {
            map.clearPath();
          }
          map.setTileColours();
        } else if (hoveredObject && gameManager.gameState == GameState.attacking) {
          Vector3 coord = new Vector3(hoveredObject.transform.position.x, hoveredObject.transform.position.y + 0.25f, hoveredObject.transform.position.z);
          Tile t = map.getTile(coord);
          if (gameManager.SelectedSkill > -1 && gameManager.SelectedPiece.GetComponent<BattleCharacter>().equippedSkills[gameManager.SelectedSkill].getTargets().Contains(t)) map.setTileColours(t);
        }
        gameManager.lineTo(gameManager.SelectedPiece);
      }

      // clicked something
      if (!EventSystem.current.IsPointerOverGameObject() && clickedObject && !gameManager.moving) {
        if (clickedObject.tag == "Cube" || clickedObject.transform.parent.tag == "Cube") {
          if (clickedObject.transform.parent.tag == "Cube") {
            clickedObject = clickedObject.transform.parent.gameObject;
          }
          // move unit to cube or attack ground
          if (gameManager.gameState == GameState.moving) {
            Vector3 selectedCoord = new Vector3(clickedObject.transform.position.x, clickedObject.transform.position.y + 1, clickedObject.transform.position.z);
            gameManager.waitToEndTurn(gameManager.movePiece(selectedCoord));
          } else if (gameManager.gameState == GameState.attacking && gameManager.SelectedSkill >= 0) {
            gameManager.attackTarget(clickedObject.GetComponent<Tile>());
          }
        } else if (clickedObject.tag == "Unit" && gameManager.gameState == GameState.attacking && gameManager.SelectedSkill >= 0) {
          // attack ground or attack unit
          gameManager.attackTarget(clickedObject.GetComponent<BattleCharacter>().curTile);
        }
      } else if (hoveredObject) {
        // show projected damage
        BattleCharacter character = hoveredObject.GetComponent<BattleCharacter>();
        gameManager.selectTarget(character);
        if (gameManager.playerTurn && !gameManager.moving) {
          if (hoveredObject.tag == "Unit") {
            // set color of hovered tile
            if (gameManager.SelectedSkill >= 0 && gameManager.SelectedPiece.GetComponent<BattleCharacter>().equippedSkills[gameManager.SelectedSkill].targetsTiles) {
              map.setTileColours(hoveredObject.GetComponent<BattleCharacter>().curTile);
            }
            // draw line to object
            gameManager.lineTo(hoveredObject);
          } else {
            // don't draw line
            gameManager.lineTo(gameManager.SelectedPiece);
          }
        }
      }
    }
  }
}

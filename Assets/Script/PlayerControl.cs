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
    Map map = gameManager.map;
    GameObject clickedObject = gameManager.getClicked(PlayerCam);
    GameObject hoveredObject = gameManager.getHovered(PlayerCam);

    if (hoveredObject && (hoveredObject.tag == "Cube" || hoveredObject.transform.parent.tag == "Cube")) {
      if (hoveredObject.transform.parent.tag == "Cube") {
        hoveredObject = hoveredObject.transform.parent.gameObject;
      }
      if (gameManager.gameState == GameState.moving) {
        Vector3 coord = new Vector3(hoveredObject.transform.position.x, hoveredObject.transform.position.y + 1, hoveredObject.transform.position.z);
        Tile t = map.getTile(coord);
        if (t.distance <= gameManager.moveRange) {
          map.setPath(coord);
          map.setTileColours();
        }
      } else if (hoveredObject && gameManager.gameState == GameState.attacking) {
        Vector3 coord = new Vector3(hoveredObject.transform.position.x, hoveredObject.transform.position.y + 0.25f, hoveredObject.transform.position.z);
        Tile t = map.getTile(coord);
        if (gameManager.SelectedSkill > -1 && gameManager.SelectedPiece.GetComponent<Character>().equippedSkills[gameManager.SelectedSkill].getTargets().Contains(t.gameObject)) map.setTileColours(t);
      }
      gameManager.lineTo(gameManager.SelectedPiece);
    }

    // clicked something
    if (!EventSystem.current.IsPointerOverGameObject() && clickedObject) {
      if (clickedObject.tag == "Cube" || clickedObject.transform.parent.tag == "Cube") {
        if (clickedObject.transform.parent.tag == "Cube") {
          clickedObject = clickedObject.transform.parent.gameObject;
        }
        // move unit to cube or attack ground
        if (gameManager.gameState == GameState.moving) {
          Vector3 selectedCoord = new Vector3(clickedObject.transform.position.x, clickedObject.transform.position.y + 1, clickedObject.transform.position.z);
          gameManager.waitToEndTurn(gameManager.MovePiece(selectedCoord));
        } else if (gameManager.gameState == GameState.attacking && gameManager.SelectedSkill >= 0) {
          gameManager.attackTarget(clickedObject);
        }
      } else if (clickedObject.tag == "Unit" && gameManager.gameState == GameState.attacking && gameManager.SelectedSkill >= 0) {
        // attack ground or attack unit
        if (gameManager.SelectedSkill >= 0 && gameManager.SelectedPiece.GetComponent<Character>().equippedSkills[gameManager.SelectedSkill].targetsTiles) {
          gameManager.attackTarget(clickedObject.GetComponent<Character>().curTile.gameObject);
        }
        else gameManager.attackTarget(clickedObject);
      }
    } else if (!gameManager.moving && gameManager.playerTurn) {
      // show projected damage
      gameManager.selectTarget(hoveredObject);
      if (hoveredObject && hoveredObject.tag == "Unit") {
        // set color of hovered tile
        if (gameManager.SelectedSkill >= 0 && gameManager.SelectedPiece.GetComponent<Character>().equippedSkills[gameManager.SelectedSkill].targetsTiles) {
          map.setTileColours(hoveredObject.GetComponent<Character>().curTile);
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

using UnityEngine;

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
    GameObject clickedObject = gameManager.getClicked(PlayerCam);
    GameObject hoveredObject = gameManager.getHovered(PlayerCam);

    // Select a piece
    if (clickedObject) {
      Vector3 selectedCoord;

      if (clickedObject.tag == "Cube") {
        if (gameManager.gameState == GameState.moving) {
          selectedCoord = new Vector3(clickedObject.transform.position.x, clickedObject.transform.position.y + 1, clickedObject.transform.position.z);
          gameManager.MovePiece(selectedCoord);
          } else if (gameManager.gameState == GameState.attacking) {
            gameManager.attackTarget(clickedObject);
          }
      } else if ((clickedObject.tag == "PiecePlayer1" || clickedObject.tag == "PiecePlayer2") && gameManager.gameState == GameState.attacking) {
        gameManager.attackTarget(clickedObject);
      }
    } else if (!gameManager.moving) {
      if (gameManager.gameState == GameState.attacking && hoveredObject) {
        gameManager.selectTarget(hoveredObject);
      }
      if (hoveredObject && (hoveredObject.tag == "PiecePlayer1" || hoveredObject.tag == "PiecePlayer2")) {
        gameManager.lineTo(hoveredObject);
      } else if (hoveredObject && hoveredObject.tag == "Cube") {
        if (gameManager.gameState == GameState.moving) {
          Vector3 coord = new Vector3(hoveredObject.transform.position.x, hoveredObject.transform.position.y + 1, hoveredObject.transform.position.z);
          Tile t = gameManager.getTile(coord);
          if (t.distance <= gameManager.SelectedPiece.GetComponent<Character>().moveRange) {
            gameManager.setPath(coord);
            gameManager.setTileColours();
            foreach (Tile ti in gameManager.path) {
              ti.gameObject.GetComponent<Renderer>().material.color = Color.blue;
            }
          }
        } else if (gameManager.gameState == GameState.attacking) {
          Vector3 coord = new Vector3(hoveredObject.transform.position.x, hoveredObject.transform.position.y + 0.25f, hoveredObject.transform.position.z);
          Tile t = gameManager.getTile(coord);
          if (gameManager.SelectedPiece.GetComponent<Character>().equippedSkills[gameManager.SelectedSkill].getTargets().Contains(t.gameObject)) gameManager.setTileColours(t);
        }
        gameManager.lineTo(gameManager.SelectedPiece);
      } else {
        gameManager.lineTo(gameManager.SelectedPiece);
      }
    }
  }
}

using UnityEngine;

public class PlayerControl : MonoBehaviour {
  private Camera PlayerCam;
  // Camera used by the player
  private GameManager gameManager;
  // GameObject responsible for the management of the game

  public int moveRange = 4;

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

    // Select a piece
    if (clickedObject) {
      Vector3 selectedCoord;

      if (clickedObject.tag == "Cube" && gameManager.gameState == GameState.moving) {
        selectedCoord = new Vector3(clickedObject.transform.position.x, clickedObject.transform.position.y + 1, clickedObject.transform.position.z);
        gameManager.MovePiece(selectedCoord);
      } else if ((clickedObject.tag == "PiecePlayer1" || clickedObject.tag == "PiecePlayer2") && gameManager.gameState == GameState.attacking) {
        gameManager.selectTarget(clickedObject);
      }
    } else if (!gameManager.moving) {
      GameObject hoveredObject = gameManager.getHovered(PlayerCam);
      if (hoveredObject && (hoveredObject.tag == "PiecePlayer1" || hoveredObject.tag == "PiecePlayer2")) {
        gameManager.lineTo(hoveredObject);
      } else if (hoveredObject && hoveredObject.tag == "Cube" && gameManager.gameState == GameState.moving) {
        gameManager.path.Clear();
        gameManager.resetTileColors();
        Vector3 coord = new Vector3(hoveredObject.transform.position.x, hoveredObject.transform.position.y + 1, hoveredObject.transform.position.z);
        Tile t = gameManager.getTile(coord);
        if (t.distance <= gameManager.SelectedPiece.GetComponent<Character>().moveRange) {
          gameManager.path.AddFirst(t);
          while (t.dir != Direction.None) {
            switch (t.dir) {
              case Direction.Forward:
                coord = coord - Vector3.forward;
                break;
              case Direction.Back:
                coord = coord - Vector3.back;
                break;
              case Direction.Left:
                coord = coord - Vector3.left;
                break;
              case Direction.Right:
                coord = coord - Vector3.right;
                break;
            }
            t = gameManager.getTile(coord);
            gameManager.path.AddFirst(t);
          }
          foreach (Tile ti in gameManager.path) {
            ti.gameObject.GetComponent<Renderer>().material.color = Color.blue;
          }
        }
        gameManager.lineTo(gameManager.SelectedPiece);
      } else {
        gameManager.lineTo(gameManager.SelectedPiece);
      }
    }
  }
}

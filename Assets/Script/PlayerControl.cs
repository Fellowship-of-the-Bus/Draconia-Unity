using UnityEngine;
using System.Collections;

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
      Debug.Log(clickedObject.tag);
      if (clickedObject.tag == ("PiecePlayer1")) {
        gameManager.SelectPiece(clickedObject);
      }

      if (gameManager.SelectedPiece) {
        Vector3 selectedCoord;

        if (clickedObject.tag == ("Cube")) {
          selectedCoord = new Vector3(clickedObject.transform.position.x, clickedObject.transform.position.y + 1, clickedObject.transform.position.z);
          gameManager.MovePiece(selectedCoord);
        }
      }
    } else {
      GameObject hoveredObject = gameManager.getHovered(PlayerCam);
      if (hoveredObject && hoveredObject.tag == "PiecePlayer1") {
        gameManager.lineTo(hoveredObject);
      } else {
        gameManager.lineTo(gameManager.SelectedPiece);
      }
    }
  }
}

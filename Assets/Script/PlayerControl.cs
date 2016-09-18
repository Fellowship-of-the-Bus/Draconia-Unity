using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {
  private Camera PlayerCam;     // Camera used by the player
  private GameManager gameManager;  // GameObject responsible for the management of the game
  
  // Use this for initialization
  void Start () {
    PlayerCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    gameManager = gameObject.GetComponent<GameManager>();
  }
  
  // Update is called once per frame
  void Update () {
    GetMouseInputs();
  }
  
  // Detect Mouse Inputs
  void GetMouseInputs() { 
    GameObject clickedObject = gameManager.getClicked(PlayerCam);

    // Select a piece
    if(clickedObject) {
      if(clickedObject.tag == ("PiecePlayer1")) {
        gameManager.SelectPiece(clickedObject);
      }

      if (gameManager.SelectedPiece) {
        Vector2 selectedCoord;
        
        if(clickedObject.tag == ("Cube")) {
          selectedCoord = new Vector2(clickedObject.transform.position.x, clickedObject.transform.position.y);
          gameManager.MovePiece(selectedCoord);
        }
      }
    }
  }
}

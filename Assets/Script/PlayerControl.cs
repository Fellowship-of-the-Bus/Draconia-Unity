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
    Ray _ray;
    RaycastHit _hitInfo;
    
    // Select a piece
    // On Left Click
    if(Input.GetMouseButtonDown(0)) {
      _ray = PlayerCam.ScreenPointToRay(Input.mousePosition); // Specify the ray to be casted from the position of the mouse click
      
      // Raycast and verify that it collided
      if(Physics.Raycast (_ray,out _hitInfo)) {
        // Select the piece if it has the good Tag
        if(_hitInfo.collider.gameObject.tag == ("PiecePlayer1")) {
          gameManager.SelectPiece(_hitInfo.collider.gameObject);
        }
      }
    }
    
    if (gameManager.SelectedPiece) {
      Vector2 selectedCoord;
      
      // On Left Click
      if(Input.GetMouseButtonDown(0)) {
        _ray = PlayerCam.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast (_ray,out _hitInfo)) {
          if(_hitInfo.collider.gameObject.tag == ("Cube")) {
            selectedCoord = new Vector2(_hitInfo.collider.gameObject.transform.position.x,_hitInfo.collider.gameObject.transform.position.y);
            gameManager.MovePiece(selectedCoord);
          }
        }
      } 
    }
  }
}

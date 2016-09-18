using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
  public GameObject SelectedPiece { get; private set; } // Selected Piece
  
  //Update SelectedPiece with the GameObject inputted to this function
  public void SelectPiece(GameObject _PieceToSelect) {
    // Change color of the selected piece to make it apparent. Put it back to white when the piece is unselected
    if (SelectedPiece) {
      SelectedPiece.GetComponent<Renderer>().material.color = Color.white;
      if (SelectedPiece == _PieceToSelect) {
        SelectedPiece = null;
        return;
      }
    }

    SelectedPiece = _PieceToSelect;
    SelectedPiece.GetComponent<Renderer>().material.color = Color.red;
  }
  
  // Move the SelectedPiece to the inputted coords
  public void MovePiece(Vector2 _coordToMove) {
    SelectedPiece.transform.position = _coordToMove;
    SelectedPiece.GetComponent<Renderer>().material.color = Color.white;
    SelectedPiece = null;
  }
}

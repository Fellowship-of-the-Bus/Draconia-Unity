using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour {
  public GameObject SelectedPiece { get; private set; }
  // Selected Piece
  List<GameObject> cubes = null;
  List<Tile> tiles = null;
  LineRenderer line;

  void Start() {
    cubes = new List<GameObject>(GameObject.FindGameObjectsWithTag("Cube"));
    tiles = new List<Tile>();
    foreach (GameObject cube in cubes) {
      cube.AddComponent<Tile>();
      Tile t = cube.GetComponent<Tile>();
      tiles.Add(t);
    }

    line = gameObject.GetComponent<LineRenderer>();
  }
    
  //Update SelectedPiece with the GameObject inputted to this function
  public void SelectPiece(GameObject _PieceToSelect) {
    // Change color of the selected piece to make it apparent. Put it back to white when the piece is unselected
    // change color of the board squares that it can move to.
    if (SelectedPiece) {
      SelectedPiece.GetComponent<Renderer>().material.color = Color.white;
      if (SelectedPiece == _PieceToSelect) {
        SelectedPiece = null;
        clearColour();
        return;
      }
    }

    SelectedPiece = _PieceToSelect;
    SelectedPiece.GetComponent<Renderer>().material.color = Color.red;
    Vector3 piecePosition = SelectedPiece.transform.position;
    line.SetPosition(0, new Vector3(piecePosition.x, 1.5f, piecePosition.z));
    line.SetPosition(1, new Vector3(piecePosition.x, 1.5f, piecePosition.z));


    Vector3 position = SelectedPiece.transform.position;
    djikstra(position);
    foreach (Tile tile in tiles) {
      if (tile.distance <= SelectedPiece.GetComponent<Player>().moveRange) {
        tile.gameObject.GetComponent<Renderer>().material.color = Color.green;
      }
    }
  }
  
  // Move the SelectedPiece to the inputted coords
  public void MovePiece(Vector3 _coordToMove) {
    Tile destination = getTile(_coordToMove);
    if (destination.distance <= SelectedPiece.GetComponent<Player>().moveRange) {
      SelectedPiece.transform.position = _coordToMove;
      SelectedPiece.GetComponent<Renderer>().material.color = Color.white;
      clearColour();
      SelectedPiece = null;
    }
  }

  // Draw line to piece
  public void lineTo(GameObject piece) {
    if (SelectedPiece && piece) {
      line.SetPosition(1, new Vector3(piece.transform.position.x, 1.5f, piece.transform.position.z));
    }
  }

  // Get the object being clicked on
  public GameObject getClicked(Camera PlayerCam) {
    if (Input.GetMouseButtonDown(0)) {
      return  getHovered(PlayerCam);
    }

    return null;
  }

  // Get the object being hovered over
  public GameObject getHovered(Camera PlayerCam) {
    Ray _ray;
    RaycastHit _hitInfo;

    _ray = PlayerCam.ScreenPointToRay(Input.mousePosition); // Specify the ray to be casted from the position of the mouse click
    // Raycast and verify that it collided
    if (Physics.Raycast(_ray, out _hitInfo)) {
      return _hitInfo.collider.gameObject;
    }

    return null;
  }

  public void djikstra(Vector3 unitLocation) {
    foreach (Tile tile in tiles) {
      tile.distance = System.Int32.MaxValue;
    }

    HashSet<Tile> tilesToGo = new HashSet<Tile>(tiles);

    Tile startTile = getTile(unitLocation);
    startTile.distance = 0;

    while (tilesToGo.Count != 0) {
      int minDistance = System.Int32.MaxValue;
      Tile minTile = null;
      foreach (Tile tile in tilesToGo) {
        if (tile.distance <= minDistance) {
          minDistance = tile.distance;
          minTile = tile;
        }
      }

      //above
      Vector3 neighbour = minTile.gameObject.transform.position + Vector3.forward;
      Tile neighbourTile = getTile(neighbour, tilesToGo); 
      if (neighbourTile != null) {
        neighbourTile.distance = minTile.distance + minTile.movePointSpent;
      }
      //below
      neighbour = minTile.gameObject.transform.position + Vector3.back;
      neighbourTile = getTile(neighbour, tilesToGo);
      if (neighbourTile != null) {
        neighbourTile.distance = minTile.distance + minTile.movePointSpent;
      }
      //right
      neighbour = minTile.gameObject.transform.position + Vector3.right;
      neighbourTile = getTile(neighbour, tilesToGo);
      if (neighbourTile != null) {
        neighbourTile.distance = minTile.distance + minTile.movePointSpent;
      }
      //left
      neighbour = minTile.gameObject.transform.position + Vector3.left;
      neighbourTile = getTile(neighbour, tilesToGo);
      if (neighbourTile != null) {
        neighbourTile.distance = minTile.distance + minTile.movePointSpent;
      }

      tilesToGo.Remove(minTile);
    }
  }


  public Tile getTile(Vector3 location) {
    return getTile(location, tiles);
  }

  public Tile getTile(Vector3 location, IEnumerable<Tile> list) {
    foreach (Tile tile in list) {
      if (Math.Abs(tile.gameObject.transform.position.x - location.x) < 0.05f &&
          Math.Abs(tile.gameObject.transform.position.z - location.z) < 0.05f) {
        return tile;
      }
    }
    return null;
  }

  public void clearColour() {
    foreach (Tile tile in tiles) {
      tile.gameObject.GetComponent<Renderer>().material.color = Color.white;
    }
  }

  public GameObject piece;

  public void createPiece() {
    Instantiate(piece);
  }
}

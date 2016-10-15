using UnityEngine;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour {
  public GameObject SelectedPiece { get; private set; }
  // Selected Piece
  List<GameObject> cubes = null;
  List<Tile> tiles = null;
  LineRenderer line;
  GameObject[] characters;
  int selectedIndex = 0;

  //EventManager
  public EventManager eventManager = new EventManager();

  void Start() {
    cubes = new List<GameObject>(GameObject.FindGameObjectsWithTag("Cube"));
    tiles = new List<Tile>();
    foreach (GameObject cube in cubes) {
      cube.AddComponent<Tile>();
      Tile t = cube.GetComponent<Tile>();
      tiles.Add(t);
    }

    line = gameObject.GetComponent<LineRenderer>();

    characters = GameObject.FindGameObjectsWithTag("PiecePlayer1");
    SelectPiece();
  }

  public void resetTileColors() {
    foreach (Tile tile in tiles) {
      if (tile.distance <= SelectedPiece.GetComponent<Player>().moveRange) {
        tile.gameObject.GetComponent<Renderer>().material.color = Color.green;
      } else {
        tile.gameObject.GetComponent<Renderer>().material.color = Color.white;
      }
    }
  }

  //Update SelectedPiece
  public void SelectPiece() {
    // Change color of the selected piece to make it apparent. Put it back to white when the piece is unselected
    // change color of the board squares that it can move to.
    clearColour();
    if (SelectedPiece) {
      SelectedPiece.GetComponent<Renderer>().material.color = Color.white;
    }

    selectedIndex = (selectedIndex + 1) % characters.Length;
    SelectedPiece = characters[(selectedIndex + 1) % characters.Length];
    SelectedPiece.GetComponent<Renderer>().material.color = Color.red;
    line.SetPosition(0, SelectedPiece.transform.position);
    line.SetPosition(1, SelectedPiece.transform.position);


    Vector3 position = SelectedPiece.transform.position;
    djikstra(position);
    resetTileColors();
  }

  // Move the SelectedPiece to the inputted coords
  public void MovePiece(Vector3 _coordToMove) {
    Tile destination = getTile(_coordToMove);
    if (destination.distance <= SelectedPiece.GetComponent<Player>().moveRange) {
      _coordToMove.y = destination.transform.position.y + getHeight(destination);
      SelectedPiece.transform.position = _coordToMove;
      SelectedPiece.GetComponent<Renderer>().material.color = Color.white;
      clearColour();
      SelectPiece();
    }
  }

  // Draw line to piece
  public void lineTo(GameObject piece) {
    if (SelectedPiece && piece) {
      if (SelectedPiece == piece) {
        line.SetPosition(1, SelectedPiece.transform.position);
      } else {
        Vector3 toTarget = piece.transform.position - SelectedPiece.transform.position;
        Ray ray = new Ray(SelectedPiece.transform.position, toTarget.normalized);
        RaycastHit hitInfo;
        Physics.Raycast(ray, out hitInfo);

        if (hitInfo.collider.gameObject == piece) {
           line.GetComponent<Renderer>().material.color = Color.red;
        } else {
          line.GetComponent<Renderer>().material.color = Color.black;
        }

        line.SetPosition(1, hitInfo.point);
      }
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
      tile.dir = Direction.None;
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

      Direction dir = Direction.None;
      //above
      Vector3 neighbour = minTile.gameObject.transform.position + Vector3.forward;
      Tile neighbourTile = getTile(neighbour, tilesToGo);
      if (neighbourTile != null) {
        int d = minTile.distance + distance(neighbourTile, minTile);
        if (d < neighbourTile.distance) {
          neighbourTile.distance = d;
          neighbourTile.dir = Direction.Forward;
        }
        //neighbourTile.distance = Math.Min(minTile.distance + distance(neighbourTile, minTile), neighbourTile.distance);
      }
      //below
      neighbour = minTile.gameObject.transform.position + Vector3.back;
      neighbourTile = getTile(neighbour, tilesToGo);
      if (neighbourTile != null) {
        int d = minTile.distance + distance(neighbourTile, minTile);
        if (d < neighbourTile.distance) {
          neighbourTile.distance = d;
          neighbourTile.dir = Direction.Back;
        }
        //neighbourTile.distance = Math.Min(minTile.distance + distance(neighbourTile, minTile), neighbourTile.distance);
      }
      //right
      neighbour = minTile.gameObject.transform.position + Vector3.right;
      neighbourTile = getTile(neighbour, tilesToGo);
      if (neighbourTile != null) {
        int d = minTile.distance + distance(neighbourTile, minTile);
        if (d < neighbourTile.distance) {
          neighbourTile.distance = d;
          neighbourTile.dir = Direction.Right;
        }
        //neighbourTile.distance = Math.Min(minTile.distance + distance(neighbourTile, minTile), neighbourTile.distance);
      }
      //left
      neighbour = minTile.gameObject.transform.position + Vector3.left;
      neighbourTile = getTile(neighbour, tilesToGo);
      if (neighbourTile != null) {
        int d = minTile.distance + distance(neighbourTile, minTile);
        if (d < neighbourTile.distance) {
          neighbourTile.distance = d;
          neighbourTile.dir = Direction.Left;
        }
        //neighbourTile.distance = Math.Min(minTile.distance + distance(neighbourTile, minTile), neighbourTile.distance);
      }
      tilesToGo.Remove(minTile);
    }
  }

  public int distance(Tile from, Tile to) {
    //check heights
    if (Math.Abs(getHeight(from) - getHeight(to)) > 1.0f) {
      return Int32.MaxValue/2;
    }
    return to.movePointSpent;
  }

  public float getHeight(Tile t) {
    float scale = t.gameObject.transform.localScale.y;
    return scale/2 + 0.5f;
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
    Instantiate(piece, new Vector3(-0.5f, 1f, 1.5f), Quaternion.identity, GameObject.FindGameObjectWithTag("ChessModels").transform);
  }
}

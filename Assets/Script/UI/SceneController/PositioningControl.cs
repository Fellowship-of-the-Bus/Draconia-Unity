using UnityEngine;
using UnityEngine.EventSystems;

public class PositioningControl : MonoBehaviour {
  BattleCharacter selectedCharacter = null;
  private Camera PlayerCam;
  // Use this for initialization
  void Awake() {
    PlayerCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

  }

  // Update is called once per frame
  void Update() {
    GetMouseInputs();
  }

  //TODO: update colouring of selected characters .....
  void deselect() {
    selectedCharacter.GetComponent<Renderer>().material.color = Color.white;
    selectedCharacter = null;
  }
  void selChar(BattleCharacter c) {
    selectedCharacter = c;
    selectedCharacter.GetComponent<Renderer>().material.color = Color.red;
  }

  //swaps locations of clicked and selected chars
  void swap(BattleCharacter clickedChar) {
    //precondition: selectedCharacter != null
    Debug.AssertFormat(selectedCharacter != null, "swap called with null selectedCharacter");

    //try to swap same characters return;
    if (clickedChar == selectedCharacter) {
      deselect();
      return;
    }

    Tile selTile = selectedCharacter.curTile;
    Tile clickTile = clickedChar.curTile;

    //swap occupants
    selTile.occupant = clickedChar;
    clickTile.occupant = selectedCharacter;

    //set curTiles
    clickedChar.curTile = selTile;
    selectedCharacter.curTile = clickTile;

    //set positions
    selectedCharacter.gameObject.transform.position = selectedCharacter.curTile.position;
    clickedChar.gameObject.transform.position = clickedChar.curTile.position;

    //deselected character
    deselect();
  }
  //places selected char at tile
  void place(Tile t) {
    //precondition: t is unoccupied
    Debug.AssertFormat(!t.occupied(), "tried to place on occupied tile");
    //check it is a valid start tile
    if (!GameSceneController.get.validStartTile(t)) return;
    //break original tile links
    selectedCharacter.curTile.occupant = null;
    selectedCharacter.curTile = null;

    //set new links
    selectedCharacter.curTile = t;
    t.occupant = selectedCharacter;

    selectedCharacter.gameObject.transform.position = t.position;

    //deselected the character
    deselect();
  }

  // Detect Mouse Inputs
  void GetMouseInputs() {
    var gameManager = GameManager.get;
    GameObject clickedObject = gameManager.getClicked(PlayerCam);
    if (clickedObject == null) {
      return;
    }
    BattleCharacter clickedChar = clickedObject.GetComponent<BattleCharacter>();
    Tile clickedTile = clickedObject.GetComponent<Tile>();

    if (clickedChar != null && clickedChar.team == 0) {
      if (clickedChar == selectedCharacter) {
        deselect();
        return;
      } else if (selectedCharacter == null) {
        selChar(clickedChar);
        return;
      } else {
        swap(clickedChar);
        return;
      }
    } else if (clickedTile != null && selectedCharacter != null) {
      //if occupied swap
      if (clickedTile.occupied()) {
        swap(clickedTile.occupant);
      } else {
        place(clickedTile);
      }
      return;
    }
  }
}

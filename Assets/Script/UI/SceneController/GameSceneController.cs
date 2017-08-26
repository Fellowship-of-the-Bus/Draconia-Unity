using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using System.Collections.Generic;

public class GameSceneController: MonoBehaviour {

  /*To make a map: set player start locations by dragging some cubes into it
   *
   */
  public Transform parent;

  public List<Tile> pStartLocTiles;
  public GameObject battleCanvas;
  public GameObject charSelectCanvas;
  public GameObject positioningCanvas;
  public PlayerControl pControl;
  public GameObject battleCanvasTooltip;
  public GameObject charSelectTooltip;

  public int numCharInBattle;

  //objective things:
  public Transform layout;
  public GameObject description;

  public void back() {
    SceneManager.LoadSceneAsync("OverWorld");
  }

  public void toCharPositioning() {
    charSelectCanvas.SetActive(false);
    positioningCanvas.SetActive(true);
  }

  public void toCharSelection() {
    charSelectCanvas.SetActive(true);
    positioningCanvas.SetActive(false);
  }

  public void startGame() {
    //switch canvas, then start the game
    battleCanvas.SetActive(true);
    charSelectCanvas.SetActive(false);
    positioningCanvas.SetActive(false);
    //enable player control
    //disable positioning control
    pControl.preview = false;
    GameManager.get.tooltip = battleCanvasTooltip;
    setStartTileColour(Color.clear);
    //need to reenable battleChars
    GameManager.get.enabled = true;
    GameManager.get.init();
    GameManager.get.startTurn();
  }
  void Awake() {
    get = this;
  }

  void Start() {

    //playerStartLocations = GameManager.get.map.getStartTiles();
    battleCanvas.SetActive(false);
    charSelectCanvas.SetActive(false);
    positioningCanvas.SetActive(true);
    pControl.preview = true;
    GameManager.get.tooltip = charSelectTooltip;

    //set colour for start locations for tile variable
    List<Tile> tileStartLocs = GameManager.get.map.getStartTiles();
    foreach (Tile t in tileStartLocs) {
      pStartLocTiles.Add(t);
    }
    resetStartTileColour();

    GameManager.get.enabled = false;

    setObjDescriptions();
  }

  public void placeCharacter (BattleCharacter c) {
    foreach(Tile t in pStartLocTiles) {
      if (!t.occupied()) {
        c.transform.position = t.position;
        t.occupant = c;
        c.curTile = t;
        return;
      }
    }
    Debug.AssertFormat(false, "Attempted to Place Character without open start location tile - This should never happen");
  }

  public void removeCharacter (BattleCharacter c) {
    Tile t = c.curTile;
    c.curTile = null;
    t.occupant = null;
    Destroy(c.gameObject);
  }

  public bool validStartTile(Tile t) {
    return pStartLocTiles.Contains(t);
  }

  private void setObjDescriptions() {
    foreach (Objective o in GameManager.get.winningConditions) {
      GameObject desc = Instantiate(description, layout);
      desc.GetComponent<Text>().text = " - " + o.description;
    }
    foreach (Objective o in GameManager.get.losingConditions) {
      GameObject desc = Instantiate(description, layout);
      desc.GetComponent<Text>().text = " - " + o.description;
    }
  }

  public void resetStartTileColour() {
    setStartTileColour(Color.red);
  }
  public void setStartTileColour(Color c) {
    List<Tile> tileStartLocs = GameManager.get.map.getStartTiles();
    foreach (Tile t in tileStartLocs) {
      t.setColor(c);
    }
  }

  public static GameSceneController get {get; set;}
}

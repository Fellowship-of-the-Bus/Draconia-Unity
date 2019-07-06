using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PostMapController: MonoBehaviour {
  public FileBrowser loadBrowser;
  public GameObject loseCanvas;
  public GameObject winCanvas;
  public GameManager.PostGameData data;

  public Transform charParent;
  public GameObject charViewPrefab;

  public AttrView attrView;
  public GameObject tipbox;

  void Start() {
    winCanvas.SetActive(false);
    loseCanvas.SetActive(false);
    data = GameManager.postData;
    if (data.win) {
      winCanvas.SetActive(true);
      displayLoot();
      foreach (Equipment e in data.loot) {
        GameData.gameData.inv.addEquipment(e);
      }

      GameData.gameData.mapProgression[data.mapName] = "";
    } else {
      loseCanvas.SetActive(true);
    }
  }

  public void displayLoot() {
    foreach (Character c in data.inBattle) {
      GameObject o = Instantiate(charViewPrefab, charParent);
      PostBattleCharViewController charView = o.GetComponent<PostBattleCharViewController>();
      charView.setCharacter(c);
    }

    // TODO: Display unlocked items and characters
  }

  public void loadGame() {
    loadBrowser.createOptions(SaveLoad.listSaveFiles());
  }

  public void loadAuto() {
    SaveLoad.loadAuto();
    LoadingScreen.load("OverWorld");
  }

  public void toOverWorld() {
    LoadingScreen.load("OverWorld");
    //autoSave when returning to overWorld.
    SaveLoad.saveAuto();
  }

  public void exit() {
    LoadingScreen.load("MainMenu");
  }
}

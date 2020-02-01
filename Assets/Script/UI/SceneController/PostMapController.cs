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
        Weapon w = e as Weapon;
        if (w != null) {
          GameData.gameData.inv.addEquipment(w);
        } else {
          GameData.gameData.inv.addEquipment(e as Armour);
        }
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

  public void retry() {
    LoadingScreen.load(data.mapName);
  }

  public void loadGame() {
    loadBrowser.createOptions(SaveLoad.listSaveFiles());
  }

  public void loadAuto() {
    LoadingScreen.load("OverWorld");
    SaveLoad.loadAuto();
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

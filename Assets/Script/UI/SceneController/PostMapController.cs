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
  public GameObject charPreview;

  void Start() {
    winCanvas.SetActive(false);
    loseCanvas.SetActive(false);
    data = GameManager.postData;
    if (data.win) {
      winCanvas.SetActive(true);
      displayLoot();
    } else {
      loseCanvas.SetActive(true);
    }
  }

  public void displayLoot() {
    foreach (Character c in data.inBattle) {
      GameObject o = Instantiate(charPreview, charParent);
      //set image and name
      string display = c.name;
      if (c.skills.numSkillPoints != 0) display += "**";
      o.GetComponentsInChildren<Text>()[0].text = display;
    }
  }
  public void loadGame() {
    loadBrowser.createOptions(SaveLoad.listSaveFiles());
  }

  public void loadAuto() {
    SaveLoad.loadAuto();
    SceneManager.LoadSceneAsync("OverWorld");
  }

  public void toOverWorld() {
    SceneManager.LoadSceneAsync("OverWorld");
    //autoSave when returning to overWorld.
    SaveLoad.saveAuto();
  }

  public void exit() {
    SceneManager.LoadSceneAsync("MainMenu");
  }
}

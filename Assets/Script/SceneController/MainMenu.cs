using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu: MonoBehaviour {
  public FileBrowser loadBrowser;

  public void test() {
    SceneManager.LoadSceneAsync("OverWorld");
  }

  public void loadGame() {
    loadBrowser.createOptions(SaveLoad.listSaveFiles());
  }

  public void newGame() {
    GameData.gameData = new GameData();
    GameData.gameData.characters.Add(new Character());
    GameData.gameData.characters.Add(new Character("Brodric"));
    SceneManager.LoadSceneAsync("OverWorld");
  }
}

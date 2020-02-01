using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu: MonoBehaviour {
  public FileBrowser loadBrowser;

  void Start() {
    SkillList.get.createDict();
    LootGenerator.init();
  }

  public void loadGame() {
    loadBrowser.createOptions(SaveLoad.listSaveFiles());
  }

  public void newGame(NewGameSettings settings) {
    GameData.gameData = new GameData();
    GameData.gameData.newGame(settings);

    LoadingScreen.load("OverWorld");
  }

  public void quitGame() {
    Application.Quit();
  }
}

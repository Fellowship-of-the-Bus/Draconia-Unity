using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu: MonoBehaviour {
  public FileBrowser loadBrowser;

  void Start() {
    SkillList.get.createDict();
    LootGenerator.init();
    Application.targetFrameRate = Options.FPS;
  }

  public void loadGame() {
    loadBrowser.openBrowser();
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

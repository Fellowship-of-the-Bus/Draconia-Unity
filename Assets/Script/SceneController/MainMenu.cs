using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu: MonoBehaviour {

  public void test() {
    SceneManager.LoadSceneAsync ("OverWorld");
  }

  public void loadGame() {
    SaveLoad.load();
    SceneManager.LoadSceneAsync ("OverWorld");
  }

  public void newGame() {
    GameData.gameData = new GameData();
    GameData.gameData.characters.Add(new Character());
    GameData.gameData.characters.Add(new Character("Brodric"));
    SceneManager.LoadSceneAsync ("OverWorld");
  }
}

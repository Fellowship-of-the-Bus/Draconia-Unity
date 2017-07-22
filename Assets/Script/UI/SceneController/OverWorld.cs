using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class OverWorld: MonoBehaviour {
  public FileBrowser saveBrowser;

  public void test() {
    SceneManager.LoadSceneAsync ("testMap");
  }
  public void manage() {
    SceneManager.LoadSceneAsync ("CharacterManagement");
  }
  public void back() {
    SceneManager.LoadSceneAsync ("MainMenu");
  }

  public void save() {
    saveBrowser.createOptions(SaveLoad.listSaveFiles());
  }

  public void inventory() {
    SceneManager.LoadSceneAsync ("Inventory");
  }

  public void option() {
    SceneManager.LoadSceneAsync("Option");
  }

}

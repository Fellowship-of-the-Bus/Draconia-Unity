using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu: MonoBehaviour {
  public MainMenu() {}

  public void test() {
    SceneManager.LoadSceneAsync ("testMap");
  }
}

using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu: MonoBehaviour {

  public void test() {
    SceneManager.LoadSceneAsync ("OverWorld");
  }
}

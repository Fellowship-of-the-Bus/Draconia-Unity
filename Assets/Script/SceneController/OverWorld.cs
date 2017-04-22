using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverWorld: MonoBehaviour {

  public void test() {
    SceneManager.LoadSceneAsync ("testMap");
  }
  public void manage() {
    SceneManager.LoadSceneAsync ("CharacterManagement");
  }
  public void back() {
    SceneManager.LoadSceneAsync ("MainMenu");
  }
}

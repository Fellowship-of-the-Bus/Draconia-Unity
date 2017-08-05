using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class OverWorld: MonoBehaviour {
  public FileBrowser saveBrowser;
  public GameObject levelParent;

  public void Start() {
    // Hide buttons for locked levels
    int levelsUnlocked = 0; // Set this in the save
    int i = 0;
    foreach(Transform child in levelParent.transform) {
      if (i > levelsUnlocked) {
        child.gameObject.SetActive(false);
      }
      i++;
    }
  }

  public void playScenario(string scenario) {
    SceneManager.LoadSceneAsync (scenario);
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

  public void skills() {
    SceneManager.LoadSceneAsync ("SkillSelect");
  }

}

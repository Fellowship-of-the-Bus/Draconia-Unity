﻿using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class OverWorld: MonoBehaviour {
  public FileBrowser saveBrowser;
  public GameObject levelParent;
  public GameObject UI;
  public Button saveButton;

  //When adding maps, key is the new map name, the value is the set of prereq maps that needs to be
  //completed already. No key => no prereqs
  private Dictionary<string, HashSet<string>> mapPrereq = new Dictionary<string, HashSet<string>>() {
    {"Map1", new HashSet<string>()},
    {"Map2", new HashSet<string>()},
    {"Harbour", new HashSet<string>()}
  };

  public void Start() {
    UI.GetComponent<CanvasGroup>().interactable = true;

    // Hide buttons for locked levels
    foreach(Transform child in levelParent.transform) {
      string mapName = child.gameObject.name;
      // Already complete Maps
      if (GameData.gameData.mapProgression.ContainsKey(mapName)) {
        if (child.gameObject.GetComponent<Tooltip>().tiptext.Contains("Campaign")) {
          child.gameObject.GetComponent<Image>().color = Color.green;
        }
      } else {
        // Check for maps that meet all prereqs
        bool prereqMet = true;
        if (mapPrereq.ContainsKey(mapName)) {
          foreach( string s in mapPrereq[mapName] ) {
            if (! GameData.gameData.mapProgression.ContainsKey(s)) {
              prereqMet = false;
              break;
            }
          }
        }
        if (prereqMet) {
          child.gameObject.GetComponent<Image>().color = Color.yellow;
        } else {
          child.gameObject.SetActive(false);
        }
      }
    }
  }

  void Update() {
    // save button should not be interactable while saving or loading
    saveButton.interactable = ! SaveLoad.active;
  }

  public void playScenario(string scenario) {
    //auto save before battle
    SaveLoad.saveAuto();
    LoadingScreen.load(scenario);
  }

  public void back() {
    LoadingScreen.load("MainMenu");
  }

  public void save() {
    saveBrowser.openBrowser();
  }
}

using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class OverWorld: MonoBehaviour {
  public FileBrowser saveBrowser;
  public GameObject levelParent;
  //When adding maps, key is the new map name, the value is the set of prereq maps that needs to be
  //completed already
  private Dictionary<string, HashSet<string>> mapPrereq = new Dictionary<string, HashSet<string>>() {
    {"Map1", new HashSet<string>()},
    {"Map2", new HashSet<string>(){"Map1"}}
  };

  public void Start() {
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
        foreach( string s in mapPrereq[mapName] ) {
          if (! GameData.gameData.mapProgression.ContainsKey(s)) {
            prereqMet = false;
            break;
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

  public void playScenario(string scenario) {
    //auto save before battle
    SaveLoad.saveAuto();

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

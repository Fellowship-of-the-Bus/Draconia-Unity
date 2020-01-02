using UnityEngine;
using System;

// Make statics visible in the editor
public class DebugDataView : MonoBehaviour {
  public GameData gameData;
  public Options.OptionData options;

  private static DebugDataView get;

  [Serializable]
  public struct SaveLoadData {
    public bool active;
    public SaveLoad.Mode mode;
  }
  public SaveLoadData saveloadData;

  void Awake() {
    if (get != null) {
      Destroy(gameObject);
      return;
    }
    get = this;
    DontDestroyOnLoad(gameObject);
  }

  void Update() {
    gameData = GameData.gameData;
    options = Options.instance;
    saveloadData.active = SaveLoad.active;
    saveloadData.mode = SaveLoad.currentMode;
  }
}

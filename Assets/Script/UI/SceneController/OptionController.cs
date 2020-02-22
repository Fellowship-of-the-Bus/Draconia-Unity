using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionController: MonoBehaviour {
  void Start() {
    gridTransparencyText.text = Options.gridTransparency.ToString();
    displayAnimationsScript.isOn = Options.displayAnimation;
    gridTransparencyScript.value = Options.gridTransparency;
  }

  public Toggle displayAnimationsScript;
  public void displayAnimations(bool newValue) {
    Options.displayAnimation = newValue;
  }

  public Text gridTransparencyText;
  public Slider gridTransparencyScript;
  public void gridTransparencyChanged(float newValue) {
    Options.gridTransparency = newValue;
    gridTransparencyText.text = newValue.ToString();
  }

  public GameObject deleteSaveDataPanel;
  public void deleteSaveDataClicked() {
    deleteSaveDataPanel.SetActive(true);
  }

  public void deleteSaveDataConfirmClicked() {
    SaveLoad.deleteAllSaveData();
    deleteSaveDataPanel.SetActive(false);
  }

  public void deleteSaveDataCancelClicked() {
    deleteSaveDataPanel.SetActive(false);
  }

  public void show() {
    gameObject.SetActive(true);
  }
}

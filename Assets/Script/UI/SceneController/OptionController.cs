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
}

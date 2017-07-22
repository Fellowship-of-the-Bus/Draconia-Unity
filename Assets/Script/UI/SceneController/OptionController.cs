using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionController: MonoBehaviour {
  public Transform parent;
  public void back() {
    SceneManager.LoadSceneAsync("OverWorld");
  }

  void Start() {
    int i = 0;
    foreach (Options.OpField field in Options.displayedOptions) {
      GameObject newPanel = Instantiate(field.panel,parent);
      field.init(newPanel);
      if (field is Options.OpToggleField) {
        Toggle t = newPanel.GetComponent<Toggle>();
        int j = i;
        t.onValueChanged.AddListener((x) => {
          onToggleChange(x, j);
        });
      } else if (field is Options.OpSlideField) {
        Slider s = ((Options.OpSlideField)field).slider;
        int j = i;
        s.onValueChanged.AddListener((x) => {
          onSliderChange((int)x, j);
        });
      }
      i++;
    }
  }

  public void onToggleChange(bool val, int location) {
    ((Options.OpToggleField)Options.displayedOptions[location]).val = val;
  }

  public void onSliderChange(int val, int location) {
    ((Options.OpSlideField)Options.displayedOptions[location]).val = val;
    ((Options.OpSlideField)Options.displayedOptions[location]).slideVal.text = val.ToString();
  }
}

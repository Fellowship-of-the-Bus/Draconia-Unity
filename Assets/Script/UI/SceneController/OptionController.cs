using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionController: MonoBehaviour {
  public Transform parent;

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
      } else if (field is Options.OpSliderField) {
        Slider s = ((Options.OpSliderField)field).slider;
        int j = i;
        s.onValueChanged.AddListener((x) => {
          onSliderChange(x, j);
        });
      }
      i++;
    }
  }

  public void onToggleChange(bool val, int location) {
    ((Options.OpToggleField)Options.displayedOptions[location]).val = val;
  }

  public void onSliderChange(float val, int location) {
    Options.OpSliderField op = (Options.OpSliderField)Options.displayedOptions[location];
    op.val = val;
    if (op.wholeValue) {
      op.slideVal.text = ((int)val).ToString();
    } else {
      op.slideVal.text = val.ToString();
    }
  }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;

public class InvCharSelect: MonoBehaviour {
  public GameObject panel;
  public Transform parent;
  public List<GameObject> panels = new List<GameObject>();
  int selectedPanelIndex = 0;
  void Start() {
    //Assumes that gameData.characters is not empty. (reasonable)
    int index = 0;
    foreach (Character c in GameData.gameData.characters) {
      GameObject charPanel = Instantiate(panel, parent);
      Text t = charPanel.transform.Find("CharName").gameObject.GetComponent<Text>();
      Image background = charPanel.transform.Find("Background").gameObject.GetComponent<Image>();
      t.text = c.name;
      //todo set image.
      Button b = charPanel.GetComponent<Button>();
      b.onClick.AddListener(() => {
        var p = charPanel;
        onButtonClick(p);
      });
      panels.Add(charPanel);

      if (index == selectedPanelIndex) {
        background.color = Color.red;
      } else {
        background.color = Color.white;
      }
      index++;
    }
  }

  public void onButtonClick(GameObject clicked){
    int i = 0;
    foreach(GameObject p in panels) {
      Image background = p.transform.Find("Background").gameObject.GetComponent<Image>();
      if (p == clicked) {
        selectedPanelIndex = i;
        background.color = Color.red;
      } else {
        background.color = Color.white;
      }
      i++;
    }
  }


}

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;


public class InvCharSelect: MonoBehaviour {
  private class Selection {
    public Text t;
    public Image background;
    public Character c;
  }

  public GameObject panel;
  public Transform parent;
  public AttrView attrView;
  //public List<Selection> panels = new List<GameObject>();
  Selection selectedPanel;
  void Start() {
    //Assumes that gameData.characters is not empty. (reasonable)
    bool firstIter = true;
    foreach (Character c in GameData.gameData.characters) {
      GameObject charPanel = Instantiate(panel, parent);
      Selection s = new Selection();
      s.t = charPanel.transform.Find("CharName").gameObject.GetComponent<Text>();
      s.background = charPanel.transform.Find("Background").gameObject.GetComponent<Image>();
      s.c = c;
      s.t.text = c.name;
      //todo set image.
      Button b = charPanel.GetComponent<Button>();
      b.onClick.AddListener(() => {
        onButtonClick(s);
      });
      if (firstIter) {
        selectedPanel = s;
        onButtonClick(s);
      }
      firstIter = false;
      //panels.Add(s);
    }
  }

  private void onButtonClick(Selection s){
    selectedPanel.background.color = Color.white;
    s.background.color = Color.red;
    selectedPanel = s;
    attrView.updateAttr(s.c.totalAttr);
  }


}

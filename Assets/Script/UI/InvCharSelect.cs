using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;


public class InvCharSelect: MonoBehaviour {
  public class Selection {
    public Text t;
    public Image background;
    public Character c;
  }

  public GameObject panel;
  public Transform parent;
  public AttrView attrView;

  public ItemTooltipSimple[] items;
  //public List<Selection> panels = new List<GameObject>();
  public Selection selectedPanel;

  void Awake() {
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
      }
      firstIter = false;
      //panels.Add(s);
    }
    get = this;
  }

  void Start() {
    onButtonClick(selectedPanel);
    foreach (ItemTooltip tooltip in items.Map((i) => i as ItemTooltip).Filter((i) => i != null)) {
      tooltip.inCharacterView = true;
      tooltip.inCombineView = false;
    }
  }

  protected virtual void onButtonClick(Selection s){
    selectedPanel.background.color = Color.clear;
    s.background.color = Color.red;
    selectedPanel = s;
    updateAttrView();
    //add new items and set up links
    foreach (Equipment e in s.c.gear) {
      items[e.type].setItem(e);
    }
  }

  public void updateAttrView () {
    attrView.updateAttr(selectedPanel.c.totalAttr);
  }


  public static InvCharSelect get { get; set; }

}

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;

public class CharSelect : MonoBehaviour {
  public GameObject panel;
  public Transform content;
  public AttrView attrView;

  public ItemTooltipSimple[] items;
  public CharPanel selectedPanel;

  void Start() {
    //Assumes that gameData.characters is not empty. (reasonable)
    bool firstIter = true;
    foreach (Character c in GameData.gameData.characters) {
      CharPanel charPanel = Instantiate(panel, content).GetComponent<CharPanel>();
      charPanel.character = c;
      //todo set image.
      charPanel.button.onClick.AddListener(() => {
        onButtonClick(charPanel);
      });
      charPanel.text.text = c.name;
      if (firstIter) {
        selectedPanel = charPanel;
        firstIter = false;
      }
    }
    onButtonClick(selectedPanel);
    foreach (ItemTooltip tooltip in items.Map(i => i as ItemTooltip).Filter(i => i != null)) {
      tooltip.inCharacterView = true;
    }
  }

  protected virtual void onButtonClick(CharPanel panel){
    selectedPanel.background.color = Color.clear;
    panel.background.color = Color.red;
    selectedPanel = panel;
    updateAttrView();
    //add new items and set up links
    foreach (Equipment e in panel.character.gear) {
      items[e.type].setItem(e);
    }
  }

  public void updateAttrView () {
    attrView.updateAttr(selectedPanel.character.totalAttr);
  }
}

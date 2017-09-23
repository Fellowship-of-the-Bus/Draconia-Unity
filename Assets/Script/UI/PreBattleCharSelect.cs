using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;


public class PreBattleCharSelect : InvCharSelect {
  void Awake() {
    get = this;
  }

  void Start() {
    //Assumes that gameData.characters is not empty. (reasonable)
    bool firstIter = true;
    foreach (Character c in GameData.gameData.characters) {
      GameObject charPanel = Instantiate(panel, parent);
      Selection s = new InvCharSelect.Selection();
      s.t = charPanel.transform.Find("CharName").gameObject.GetComponent<Text>();
      s.background = charPanel.transform.Find("Background").gameObject.GetComponent<Image>();
      Button selectButton = charPanel.transform.Find("SelectButton").gameObject.GetComponent<Button>();
      s.c = c;
      s.t.text = c.name;
      //todo set image.
      Button b = charPanel.GetComponent<Button>();
      b.onClick.AddListener(() => {
        onButtonClick(s);
      });
      Character curChar = c;
      selectButton.onClick.AddListener(() => {
        CharIntoLevel.get.addCharacter(curChar);
      });
      if (firstIter) {
        selectedPanel = s;
      }
      firstIter = false;
      //panels.Add(s);
    }
    onButtonClick(selectedPanel);
    foreach (ItemTooltip tooltip in items.Map((i) => i as ItemTooltip).Filter((i) => i != null)) {
      tooltip.inCharacterView = true;
      tooltip.inCombineView = false;
    }
  }

  public new static PreBattleCharSelect get { get; set; }

}

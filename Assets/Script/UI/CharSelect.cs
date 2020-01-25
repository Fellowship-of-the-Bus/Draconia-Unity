using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;

public class CharSelect : MonoBehaviour {
  public GameObject panel;
  public Transform content;
  public AttrView attrView;

  public Item[] items;
  public CharPanel selectedPanel {
    get { return selections[curSelection]; }
  }
  public SkillSelectController skillSelectController;

  private int curSelection = 0;
  private CharPanel[] selections;

  void Start() {
    //Assumes that gameData.characters is not empty. (reasonable)
    int numCharacters = GameData.gameData.characters.Count;
    selections = new CharPanel[numCharacters];
    for (int index = 0; index < numCharacters; ++index) {
      Character character = GameData.gameData.characters[index];
      CharPanel charPanel = Instantiate(panel, content).GetComponent<CharPanel>();
      charPanel.gameObject.name = character.name;
      charPanel.character = character;
      int curIndex = index;
      //todo set image.
      charPanel.button.onClick.AddListener(() => {
        onButtonClick(curIndex);
      });
      charPanel.text.text = character.name;
      selections[index] = charPanel;
    }
    onButtonClick(curSelection);
  }

  // enable scrolling through characters with the vertical axis
  private const float fireDelta = 0.25F;
  private float curTime = 0.0F;
  void Update() {
    curTime += Time.deltaTime;
    if (curTime > fireDelta) {

      float vert = Input.GetAxis("Vertical");
      if (vert > 0) {
        curTime = 0.0F;
        prevSelection();
      } else if (vert < 0) {
        curTime = 0.0F;
        nextSelection();
      }
    }
  }

  private void onButtonClick(int index) {
    selectedPanel.background.color = Color.clear;
    curSelection = index;
    selectedPanel.background.color = Color.red;
    updateAttrView();
    //add new items and set up links
    foreach (Equipment e in selectedPanel.character.gear) {
      items[e.type].equipment = e;
    }
    skillSelectController.setChar(selectedPanel.character);
  }

  private void prevSelection() {
    if (curSelection > 0) {
      onButtonClick(curSelection-1);
    }
  }

  private void nextSelection() {
    if (curSelection+1 < selections.Length) {
      onButtonClick(curSelection+1);
    }
  }

  public void updateAttrView () {
    attrView.updateAttr(selectedPanel.character.totalAttr);
  }
}

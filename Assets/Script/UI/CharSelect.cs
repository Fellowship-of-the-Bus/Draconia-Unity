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

  private int curSelection = 0;
  private CharPanel[] selections;

  private ScrollInputTimer scrollTimer;

  public delegate void OnCharacterChangeHandler(Character oldCharacter, Character newCharacter);
  public event OnCharacterChangeHandler onCharacterChange;

  void Awake() {
    scrollTimer = new ScrollInputTimer(this);
  }

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
  private class ScrollInputTimer : Timer {
    private CharSelect charSelect;

    public ScrollInputTimer(CharSelect charSelect) : base(0.25f) {
      this.charSelect = charSelect;
    }

    protected override bool Fire() {
      float vert = Input.GetAxis("Vertical");
      if (vert > 0) {
        charSelect.prevSelection();
        return true;
      } else if (vert < 0) {
        charSelect.nextSelection();
        return true;
      }
      return false;
    }
  }

  void Update() {
    scrollTimer.Update();
  }

  private void onButtonClick(int index) {
    Character oldCharacter = selectedPanel.character;
    selectedPanel.background.color = Color.clear;
    curSelection = index;
    Character newCharacter = selectedPanel.character;
    selectedPanel.background.color = Color.red;
    updateAttrView();
    //add new items and set up links
    foreach (Equipment e in newCharacter.gear) {
      items[e.type].equipment = e;
    }
    onCharacterChange?.Invoke(oldCharacter, newCharacter);
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

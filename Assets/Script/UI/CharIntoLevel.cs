using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;


public class CharIntoLevel: MonoBehaviour {

  public GameObject panel;
  public Transform parent;
  public Text display;

  public List<Character> addedCharacters = new List<Character>();
  CharPreview curPreview;

  private int _numCharSelected;
  public int numCharSelected {
    get {return _numCharSelected;}
    set { _numCharSelected = value; setText(value);}
  }

  public CharPreview selectedPanel;

  void Awake() {
    get = this;
  }

  void Start() {
    numCharSelected = 0;
  }

  protected virtual void onButtonClick(CharPreview preview){
    if (curPreview != null) {
      curPreview.background.color = Color.white;
    }
    preview.background.color = Color.red;
    curPreview = preview;
  }

  public void addCharacter() {
    InvCharSelect inv = InvCharSelect.get;
    Character c = inv.selectedPanel.c;
    //check character has not been added
    if (addedCharacters.Contains(c)) {
      return;
    }
    //check we are not over limit
    if (numCharSelected >= GameSceneController.get.numCharInBattle) {
      return;
    }

    BattleCharacter newBattleChar = GameManager.get.createPiece();
    newBattleChar.init();
    newBattleChar.baseChar = c;
    GameSceneController.get.placeCharacter(newBattleChar);

    //instantiate a new panel
    GameObject o = Instantiate(panel, parent);
    CharPreview preview = o.GetComponent<CharPreview>();
    preview.init(newBattleChar);
    //add listener
    o.GetComponent<Button>().onClick.AddListener(() => {
        onButtonClick(preview);
    });

    addedCharacters.Add(c);
    setText(addedCharacters.Count);
  }

  public void removeCharacter() {
    if (curPreview == null) return;
    GameSceneController.get.removeCharacter(curPreview.c);
    curPreview.gameObject.transform.SetParent(null, false);
    addedCharacters.Remove(curPreview.c.baseChar);
    curPreview = null;
    setText(addedCharacters.Count);
  }


  protected void setText(int i) {
    display.text = "Characters Selected: " + i +"/" + GameSceneController.get.numCharInBattle + ".";
  }
  public static CharIntoLevel get { get; set; }

}

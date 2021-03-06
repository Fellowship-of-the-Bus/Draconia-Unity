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
  //CharPreview curPreview;

  public int numCharSelected {
    get {return addedCharacters.Count;}
    set {setText(value);}
  }

  public CharPreview selectedPanel;

  void Awake() {
    get = this;
  }

  void Start() {
    numCharSelected = 0;
  }

  // protected virtual void onButtonClick(CharPreview preview){
  //   if (curPreview != null) {
  //     curPreview.background.color = Color.white;
  //   }
  //   preview.background.color = Color.red;
  //   curPreview = preview;
  // }

  public bool canAddCharacter(Character c) {
    return !addedCharacters.Contains(c);
  }

  public void addCharacter(Character c, bool mandatory = false) {
    //check character has not been added
    if (!canAddCharacter(c)) {
      return;
    }
    //check we are not over limit
    if (numCharSelected >= GameSceneController.get.numChars()) {
      return;
    }

    BattleCharacter newBattleChar = GameManager.get.createPiece();
    newBattleChar.baseChar = c;
    newBattleChar.init();
    GameSceneController.get.placeCharacter(newBattleChar);

    //instantiate a new panel
    GameObject o = Instantiate(panel, parent);
    CharPreview preview = o.GetComponent<CharPreview>();
    preview.init(newBattleChar);
    //add listener
    // o.GetComponent<Button>().onClick.AddListener(() => {
    //   onButtonClick(preview);
    // });
    Button unselect = o.transform.Find("Unselect").gameObject.GetComponent<Button>();
    unselect.onClick.AddListener(() => {
      removeCharacter(preview);
    });
    if (mandatory) {
      o.transform.Find("Unselect").gameObject.SetActive(false);
    }


    addedCharacters.Add(c);
    numCharSelected = addedCharacters.Count;
  }

  public void removeCharacter(CharPreview curPreview) {
    //if (curPreview == null) return;
    GameSceneController.get.removeCharacter(curPreview.character);
    curPreview.gameObject.transform.SetParent(null, false);
    addedCharacters.Remove(curPreview.character.baseChar);
    curPreview = null;
    numCharSelected = addedCharacters.Count;
  }


  protected void setText(int i) {
    display.text = "Characters Selected: " + i +"/" + GameSceneController.get.numChars() + ".";
  }
  public static CharIntoLevel get { get; set; }

}

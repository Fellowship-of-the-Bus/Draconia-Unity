using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;

public class InventoryController : MonoBehaviour {
  public CharSelect charSelect;

  public class Selection {
    public Text text;
    public Image background;
    public Character character;
  }

  public GameObject panel;
  public Transform parent;
  public AttrView attrView;

  public Item[] items;
  public Selection selectedPanel;

  public Text equipName;
  public Text equippedTo;
  public Tooltip tooltip;
  public GameObject tipbox;


  void Awake() {
    //Assumes that gameData.characters is not empty. (reasonable)
    bool firstIter = true;
    foreach (Character character in GameData.gameData.characters) {
      CharPanel charPanel = Instantiate(panel, parent).GetComponent<CharPanel>();
      Selection sel = new Selection();
      sel.text = charPanel.text;
      sel.background = charPanel.background;
      sel.character = character;
      sel.text.text = character.name;
      //todo set image.
      charPanel.button.onClick.AddListener(() => {
        onButtonClick(sel);
      });
      if (firstIter) {
        selectedPanel = sel;
        firstIter = false;
      }
    }
  }

  void Start() {
    onButtonClick(selectedPanel);
  }

  protected virtual void onButtonClick(Selection newSelection){
    selectedPanel.background.color = Color.clear;
    newSelection.background.color = Color.red;
    selectedPanel = newSelection;
    updateAttrView();
    //add new items and set up links
    foreach (Equipment e in newSelection.character.gear) {
      items[e.type] = getItem(e);
    }
  }

  public void equip(Equipment equipment) {
    var item = charSelect.items[equipment.type];
    var currentlyEquipped = item.equipment;
    if (currentlyEquipped != null && ! currentlyEquipped.isDefaultEquipment) {
      getItem(currentlyEquipped).updateColor();
    }
    charSelect.selectedPanel.character.equip(equipment);
    item.equipment = equipment;
    item.updateColor();
    updateAttrView();
  }

  private Item getItem(Equipment eq) {
    for (int i = 0; i < items.Length; ++i) {
      if (items[i].equipment == eq) {
        return items[i];
      }
    }
    Debug.AssertFormat(false, "Could not find equipment in inventory: {0}", eq);
    return null;
  }

  public void updateAttrView () {
    attrView.updateAttr(selectedPanel.character.totalAttr);
  }
}
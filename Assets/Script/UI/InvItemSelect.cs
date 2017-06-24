using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;


public class InvItemSelect: MonoBehaviour {

  public GameObject panel;
  public Transform parent;
  void Awake() {
    //Assumes that gameData.characters is not empty. (reasonable)
    foreach (Equipment e in GameData.gameData.inv.equipments) {
      Instantiate(panel, parent);
    }
    get = this;
  }
  void Start() {
    int index=0;
    foreach (Equipment e in GameData.gameData.inv.equipments) {
      var equipPanel = parent.GetChild(index).gameObject;
      ItemTooltip tooltip = equipPanel.GetComponent<ItemTooltip>();
      tooltip.setItem(e);
      InvCharSelect charSelect = InvCharSelect.get;
      if (e == charSelect.items[e.type].equip) {
        tooltip.linkedTo = charSelect.items[e.type];
        charSelect.items[e.type].linkedTo = tooltip;
      }
      tooltip.updateColour();
      index++;
    }


  }

  public ItemTooltip getTooltipWithEquipment(Equipment e) {
    int index = 0;
    foreach (Equipment equip in GameData.gameData.inv.equipments) {
      if (equip == e) {
        break;
      }
      index++;
    }
    return parent.GetChild(index).gameObject.GetComponent<ItemTooltip>();
  }

  public static InvItemSelect get { get; set; }

}

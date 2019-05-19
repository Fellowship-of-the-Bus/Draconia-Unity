using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Linq;

public class InvItemSelect: MonoBehaviour {
  // prefabs
  public GameObject equipPanel;

  // actual objects
  private GameObject currentScrollView; // currently visible scroll view
  public GameObject scrollArea; // content area

  private LinkedList<Equipment> ownedEquipment;


  void Awake() {
    ownedEquipment = GameData.gameData.inv.equipments;

    get = this;
  }

  void Start() {
    int eqIndex = 0;
    foreach (Equipment e in ownedEquipment) {
      // create an item tooltip for each piece of equipment
      var equipPanel = scrollArea.transform.GetChild(eqIndex).gameObject;
      ItemTooltip tooltip = equipPanel.GetComponent<ItemTooltip>();
      tooltip.inCharacterView = false;
      tooltip.setItem(e);
      InvCharSelect charSelect = InvCharSelect.get;
      tooltip.updateColour();
      eqIndex++;
    }
  }

  public ItemTooltip getTooltipWithEquipment(Equipment e) {
    // if (e == null) return null;
    // int index = groupedEquipment[e.equipmentClass].IndexOf(e);
    // if (index == -1) return null;
    return scrollArea.transform.GetChild(0).gameObject.GetComponent<ItemTooltip>();
  }

  public static InvItemSelect get { get; set; }
}

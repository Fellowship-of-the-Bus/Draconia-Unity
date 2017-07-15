using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;


public class InvItemSelect: MonoBehaviour {

  public GameObject panel;
  public Transform parent;

  public ItemTooltip[] items;
  private int numMaterials = 3;
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
      tooltip.inCharacterView = false;
      tooltip.inCombineView = false;
      tooltip.setItem(e);
      InvCharSelect charSelect = InvCharSelect.get;
      tooltip.updateColour();
      index++;
    }

    foreach (ItemTooltip tooltip in items) {
      tooltip.inCharacterView = false;
      tooltip.inCombineView = true;
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

  //called to add an item as a material
  public void addMaterial(Equipment equip) {
    //check we do not have this already
    for (int i = 0; i< numMaterials; i++) {
      if (items[i].equip == equip) {
        return;
      }
    }
    //add it to an empty slot
    bool added = false;
    for(int i = 0; i < numMaterials; i++) {
      if (items[i].equip == null) {
        items[i].setItem(equip);
        added = true;
        break;
      }
    }

    //check we have 3 materials
    for (int i = 0; i < numMaterials; i++) {
      if (items[i].equip == null) {
        return;
      }
    }
    //if we added the third material
    if (added) {
      generateUpgrade();
    }
  }

  public bool isResult(ItemTooltip tooltip) {
    return tooltip == items[numMaterials];
  }
  //called when 3 items are put into slots
  public void generateUpgrade() {
    //check items are compatible
    //for now check for same tier and type (weapon, armour)
    int tier = -1;
    EquipType type = null;
    for (int i = 0; i< numMaterials; i++) {
      if (tier == -1) {
        tier = items[i].equip.tier;
        type = items[i].equip.type;
      } else if (!(tier == items[i].equip.tier && type == items[i].equip.type)) {
        return;
      }
    }

    Equipment e = GameData.gameData.inv.combine(items[0].equip, items[1].equip, items[2].equip);

    //add new equipment to created slot to preview
    AttrView view = InventoryController.get.attrView;
    Text equipName = InventoryController.get.equipName;
    Text equippedTo = InventoryController.get.equippedTo;

    //make sure this does not change the attribute view
    Attributes attr = view.curAttr;
    string name = equipName.text;
    string charaName = equippedTo.text;

    items[numMaterials].setItem(e);

    view.updateAttr(attr);
    equipName.text = name;
    equippedTo.text = charaName;
  }

  //called when actually creating item
  public void createUpgrade() {
    //remove each tooltip from the inventory view
    Equipment material1 = items[0].equip;
    Equipment material2 = items[1].equip;
    Equipment material3 = items[2].equip;
    List<ItemTooltip> toDelete = new List<ItemTooltip>();
    foreach (Equipment e in GameData.gameData.inv.equipments) {
      if (e == material1 || e == material2 || e == material3) {
        toDelete.Add(getTooltipWithEquipment(e));
      }
    }
    foreach (ItemTooltip tooltip in toDelete) {
      //detach from parent
      tooltip.gameObject.transform.SetParent(null);
      Destroy(tooltip.gameObject);
    }

    //remove from inventory
    deleteEquipment(material1);
    deleteEquipment(material2);
    deleteEquipment(material3);

    Equipment created = items[numMaterials].equip;

    GameData.gameData.inv.addEquipment(created);
    //make new tooltip
    GameObject display = Instantiate(panel, parent);
    ItemTooltip newTooltip = display.GetComponent<ItemTooltip>();
    newTooltip.inCharacterView = false;
    newTooltip.inCombineView = false;
    newTooltip.equip = created;
    newTooltip.updateColour();
    //reset materials tooltips
    foreach (ItemTooltip tooltip in items) {
      tooltip.setItem(null);
    }
  }

  private void deleteEquipment(Equipment e) {
    if (e.equippedTo != null) {
      Character c = e.equippedTo;
      Equipment def = e.getDefault();
      c.equip(def);
      if (c == InvCharSelect.get.selectedPanel.c){
        InvCharSelect.get.items[e.type].equip = def;
      }
    }
    GameData.gameData.inv.deleteEquipment(e);

  }

  public void removeCreated() {
    items[numMaterials].equip = null;
  }

  public static InvItemSelect get { get; set; }

}

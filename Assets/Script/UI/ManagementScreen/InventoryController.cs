using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;

public class InventoryController : MonoBehaviour {
  public CharSelect charSelect;
  public GameObject itemPrefab;
  public Transform content;

  public List<Item> items;

  private void addItems<T>(List<T> equips) where T : Equipment {
    for (int i = 0; i < equips.Count; ++i) {
      int idx = i;
      Item item = Instantiate(itemPrefab, content).GetComponent<Item>();
      item.equipment = equips[i];
      item.button.onClick.AddListener(() => equip(equips[idx]));
      items.Add(item);
    }    
  }

  void Awake() {
    List<Weapon> weapons = GameData.gameData.inv.weapons;
    List<Armour> armour = GameData.gameData.inv.armour;
    items = new List<Item>();
    items.Capacity = weapons.Count + armour.Count;
    addItems(weapons);
    addItems(armour);
  }

  public void equip(Equipment equipment) {
    var item = charSelect.items[equipment.type];
    var currentlyEquipped = item.equipment;
    if (currentlyEquipped != null && ! currentlyEquipped.isDefaultEquipment) {
      getItem(currentlyEquipped).updateColor(Color.white);
    }
    charSelect.selectedPanel.character.equip(equipment);
    item.equipment = equipment;
    getItem(equipment).updateColor(Color.green);
    charSelect.updateAttrView();
  }

  private Item getItem(Equipment eq) {
    for (int i = 0; i < items.Count; ++i) {
      if (items[i].equipment == eq) {
        return items[i];
      }
    }
    Debug.AssertFormat(false, "Could not find equipment in inventory: {0}", eq.name());
    return null;
  }
}

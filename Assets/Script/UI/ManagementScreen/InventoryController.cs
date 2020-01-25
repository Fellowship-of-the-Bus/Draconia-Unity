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

  public readonly Color selectedColor = Color.green;
  public readonly Color unselectedColor = Color.white;

  private void addItems<T>(List<T> equips) where T : Equipment {
    for (int i = 0; i < equips.Count; ++i) {
      Item item = Instantiate(itemPrefab, content).GetComponent<Item>();
      Equipment equipment = equips[i];
      item.equipment = equipment;
      item.button.onClick.AddListener(() => equip(equipment));
      item.tooltip.tiptext = equipment.tooltip;
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
    charSelect.onCharacterChange += characterChanged;
  }

  public void equip(Equipment equipment) {
    var item = charSelect.items[equipment.type];
    var currentlyEquipped = item.equipment;
    Debug.Assert(currentlyEquipped != null);
    getItem(currentlyEquipped).updateColor(unselectedColor);
    charSelect.selectedPanel.character.equip(equipment);
    item.equipment = equipment;
    getItem(equipment).updateColor(selectedColor);
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

  private void characterChanged(Character oldCharacter, Character newCharacter) {
    foreach (Equipment e in oldCharacter.gear) {
      getItem(e).updateColor(unselectedColor);
    }
    foreach (Equipment e in newCharacter.gear) {
      getItem(e).updateColor(selectedColor);
    }
  }
}

﻿using UnityEngine;
using System;
using System.Collections.Generic;

public enum EquipmentClass {
  Sword, Bow, Axe, // Weapon
  Shield, Metal, Robe, Leather, // Armor
  Unarmed // Unarmed works for either, so keep it last
}

public static class EquipmentClassMethods {
  public static Weapon.Kinds getWeaponKind(this EquipmentClass e) {
    switch (e) {
      case EquipmentClass.Sword:
      case EquipmentClass.Axe:
      case EquipmentClass.Unarmed:
        return Weapon.Kinds.Melee;
      case EquipmentClass.Bow:
        return Weapon.Kinds.Ranged;
      default:
        Debug.AssertFormat(false, "getWeaponKind called with non-weapon EquipmentClass: {0}", e);
        return Weapon.Kinds.Melee;
    }
  }
}

[System.Serializable]
public abstract class Equipment {
  string[] tierName = new string[]{"Crude", "Simple", "Sturdy", "Quality", "Flawless", "Enchanted"};
  public Attributes attr = new Attributes();
  public int tier;
  //could be null if not equipped.
  public Character equippedTo;

  public EquipmentClass equipmentClass;

  public bool isDefaultEquipment { get { return equipmentClass == EquipmentClass.Unarmed; } }

  // return the default of the same type as e
  public abstract Equipment getDefault();

  public abstract Equipment upgrade(Equipment e1, Equipment e2);

  public string name() {
    return tierName[tier] + " " + equipmentClass;
  }

  public bool canUpgrade() {
    return tier < 5;
  }

  public EquipType type;
}

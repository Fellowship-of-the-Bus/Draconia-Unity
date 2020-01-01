using UnityEngine;
using System;

public enum EquipmentClass {
  Sword, Bow, Axe, Staff, Spear, // Weapon
  Shield, Metal, Robe, Leather, // Armor
  Unarmed // Unarmed works for either, so keep it last
} // MUST keep getWeaponKind consistent with this enum

public static class EquipmentClassMethods {
  public static Weapon.Kind getWeaponKind(this EquipmentClass e) {
    switch (e) {
      case EquipmentClass.Sword:
      case EquipmentClass.Axe:
      case EquipmentClass.Spear:
      case EquipmentClass.Staff:
      case EquipmentClass.Unarmed:
        return Weapon.Kind.Melee;
      case EquipmentClass.Bow:
        return Weapon.Kind.Ranged;
      default:
        Debug.AssertFormat(false, "getWeaponKind called with non-weapon EquipmentClass: {0}", e);
        return Weapon.Kind.Melee;
    }
  }
}

[Serializable]
public abstract class Equipment {
  public enum Tier {
    Crude, Simple, Sturdy, Quality, Flawless, Enchanted
  }

  public Attributes attr = new Attributes();
  public int tier;

  //could be null if not equipped.
  [Obsolete("Equipment.equippedTo is deprecated. Equipment is being deduplicated.")]
  public Character equippedTo {
    get {
      return GameData.gameData.getCharacterWithItem(this);
    }
  }

  public EquipmentClass equipmentClass;

  public bool isDefaultEquipment { get { return equipmentClass == EquipmentClass.Unarmed; } }

  // return the default of the same type as e
  public abstract Equipment getDefault();

  public abstract Equipment upgrade(Equipment e1, Equipment e2);

  public string name() {
    return tier.ToString() + " " + equipmentClass;
  }

  [Obsolete("Equipment.isEquipped is deprecated. Equipment is being deduplicated.")]
  public bool isEquipped() {
     return equippedTo != null;
  }

  public bool canUpgrade() {
    return (int)tier < (int)Tier.Enchanted;
  }

  public Equipment clone() {
    return MemberwiseClone() as Equipment;
  }

  public EquipType type;
}

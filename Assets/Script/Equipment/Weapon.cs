using UnityEngine;
using UnityEngine.Serialization;
using System;
using System.Collections.Generic;

public static class WeaponModels {
  static GameObject Sword = Resources.Load("Sword") as GameObject;
  public static GameObject Axe = Resources.Load("Axe") as GameObject;
  // static GameObject Jumonji = Resources.Load("jumonji") as GameObject;
  static GameObject Spear = Resources.Load("yari") as GameObject;
  static GameObject Bow = Resources.Load("BowPrefab") as GameObject;
  static GameObject Staff = Resources.Load("Staff") as GameObject;

  public static Dictionary<Weapon.EquipmentClass,GameObject> weaponModels = new Dictionary<Weapon.EquipmentClass,GameObject>() {
    {Weapon.EquipmentClass.Unarmed, null},
    {Weapon.EquipmentClass.Sword, Sword},
    {Weapon.EquipmentClass.Axe, Axe},
    {Weapon.EquipmentClass.Bow, Bow},
    {Weapon.EquipmentClass.Staff, Staff},
    {Weapon.EquipmentClass.Spear, Spear},
  };
}

[System.Serializable]
public class Weapon : Equipment {
  public enum Kind { Melee, Ranged };
  public enum EquipmentClass {
    Sword, Bow, Axe, Staff, Spear, Unarmed
  } // MUST keep getWeaponKind consistent with this enum

  public int range = 1;

  [FormerlySerializedAs("newEquipmentClass")]
  public EquipmentClass equipmentClass;

  public Kind kind {
    get {
      return equipmentClass.getWeaponKind();
    }
  }

  public static Weapon defaultWeapon {
    get { return new Weapon(EquipmentDB.get.weapons[0]); }
  }

  public GameObject getModel() {
    return WeaponModels.weaponModels[equipmentClass];
  }

  public override Equipment getDefault() { return defaultWeapon; }

  public Weapon(WeaponData weaponData) {
    this.guid = weaponData.guid;
    this.type = EquipType.weapon;
    this.equipmentClass = weaponData.equipmentClass;
    this.range = weaponData.range;
    this.tier = weaponData.tier;
  }

  public override string name() {
    return tier.ToString() + " " + equipmentClass;
  }
}

public static class EquipmentClassMethods {
  public static Weapon.Kind getWeaponKind(this Weapon.EquipmentClass e) {
    switch (e) {
      case Weapon.EquipmentClass.Sword:
      case Weapon.EquipmentClass.Axe:
      case Weapon.EquipmentClass.Spear:
      case Weapon.EquipmentClass.Staff:
      case Weapon.EquipmentClass.Unarmed:
        return Weapon.Kind.Melee;
      case Weapon.EquipmentClass.Bow:
        return Weapon.Kind.Ranged;
      default:
        Debug.AssertFormat(false, "getWeaponKind called with non-weapon EquipmentClass: {0}", e);
        return Weapon.Kind.Melee;
    }
  }
}

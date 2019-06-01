using UnityEngine;
using System;
using System.Collections.Generic;

public static class WeaponModels {
  static GameObject Sword = Resources.Load("Sword") as GameObject;
  public static GameObject Axe = Resources.Load("Axe") as GameObject;
  // static GameObject Jumonji = Resources.Load("jumonji") as GameObject;
  static GameObject Spear = Resources.Load("yari") as GameObject;
  static GameObject Bow = Resources.Load("Bow") as GameObject;
  static GameObject Staff = Resources.Load("Staff") as GameObject;

  public static Dictionary<EquipmentClass,GameObject> weaponModels = new Dictionary<EquipmentClass,GameObject>() {
    {EquipmentClass.Unarmed, null},
    {EquipmentClass.Sword, Sword},
    {EquipmentClass.Axe, Axe},
    {EquipmentClass.Bow, Bow},
    {EquipmentClass.Staff, Staff},
    {EquipmentClass.Spear, Spear},
  };
}

[System.Serializable]
public class Weapon : Equipment {
  public int range = 1;
  public enum Kinds { Melee, Ranged };
  public Kinds kind {
    get {
      return equipmentClass.getWeaponKind();
    }
  }

  public static Weapon defaultWeapon {
    get { return new Weapon(EquipmentClass.Unarmed, 1, 1); }
  }

  public GameObject getModel() {
    return WeaponModels.weaponModels[equipmentClass];
  }

  public override Equipment getDefault() { return defaultWeapon; }

  public override Equipment upgrade(Equipment e1, Equipment e2) {
    return new Weapon(equipmentClass, range, tier);
  }

  public Weapon(EquipmentClass equipmentClass, int range, int tier) {
    this.type = EquipType.weapon;
    this.equipmentClass = equipmentClass;
    this.range = range;
    this.tier = tier;
  }
}

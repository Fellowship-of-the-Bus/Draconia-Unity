using UnityEngine;
using System;
using System.Collections.Generic;

public static class WeaponModels {
  static GameObject Sword = Resources.Load("Sword") as GameObject;
  // static GameObject Jumonji = Resources.Load("jumonji") as GameObject;
  // static GameObject Yari = Resources.Load("yari") as GameObject;
  static GameObject Bow = Resources.Load("Bow") as GameObject;
  static GameObject Staff = Resources.Load("Staff") as GameObject;

  public static Dictionary<EquipmentClass,GameObject> weaponModels = new Dictionary<EquipmentClass,GameObject>() {
    {EquipmentClass.Unarmed, null},
    {EquipmentClass.Sword, Sword},
    {EquipmentClass.Bow, Bow},
    {EquipmentClass.Staff, Staff},
  };
}

[System.Serializable]
public class Weapon : Equipment {
  public int range = 1;
  public enum Kinds { Melee, Ranged, Default };
  public Kinds kind = Kinds.Default;

  public static Weapon defaultWeapon {
    get { return new Weapon(EquipmentClass.Unarmed, 1, 1); }
  }

  public GameObject getModel() {
    return WeaponModels.weaponModels[equipmentClass];
  }

  public override Equipment getDefault() { return defaultWeapon; }

  public override Equipment upgrade(Equipment e1, Equipment e2) {
    return new Weapon(equipmentClass, range, tier, kind);
  }

  public Weapon(EquipmentClass equipmentClass, int range, int tier, Kinds kind = Kinds.Default) {
    this.type = EquipType.weapon;
    this.equipmentClass = equipmentClass;
    this.range = range;
    this.tier = tier;
    if (kind == Kinds.Default) {
      this.kind = equipmentClass.getWeaponKind();
    } else {
      this.kind = kind;
    }
  }

  /*public Weapon(kinds k) {
    range = k == Ranged ? 3 : 1;
    kind = k;
  }*/
}

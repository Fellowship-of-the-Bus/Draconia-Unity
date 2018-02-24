using UnityEngine;
using System;
using System.Collections.Generic;

[System.Serializable]
public class Weapon : Equipment {
  public int range = 1;
  public enum Kinds { Melee, Ranged, Default };
  public Kinds kind = Kinds.Default;

  public static Weapon defaultWeapon {
    get { return new Weapon(EquipmentClass.Bow, 1, 1); }
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

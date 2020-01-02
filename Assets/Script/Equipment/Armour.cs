using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Armour : Equipment {
  public EquipmentClass newEquipmentClass;

  public enum EquipmentClass {
    Shield, Metal, Robe, Leather, Unarmed
  }

  public static Armour defaultArmour {
    get { return new Armour(global::EquipmentClass.Unarmed, 1); }
  }

  public override Equipment getDefault() { return defaultArmour; }

  public override Equipment upgrade(Equipment e1, Equipment e2) {
    return new Armour(equipmentClass, tier);
  }

  public Armour(global::EquipmentClass equipmentClass, int tier) {
    this.equipmentClass = equipmentClass;
    this.tier = tier;
    this.type = EquipType.armour;
  }
}

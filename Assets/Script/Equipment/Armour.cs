using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Armour : Equipment {
  public static readonly Armour defaultArmour = new Armour(EquipmentClass.Unarmed, 1);

  public override Equipment getDefault() { return defaultArmour; }

  public override Equipment upgrade(Equipment e1, Equipment e2) {
    return new Armour(equipmentClass, tier);
  }

  public Armour(EquipmentClass equipmentClass, int tier) {
    this.equipmentClass = equipmentClass;
    this.tier = tier;
    this.type = EquipType.armour;
  }
}

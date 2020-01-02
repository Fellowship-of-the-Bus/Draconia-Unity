using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;

[System.Serializable]
public class Armour : Equipment {
  public EquipmentClass equipmentClass { 
    get { return newEquipmentClass; } 
    set { newEquipmentClass = value; }
  }

  // [FormerlySerializedAs("newEquipmentClass")]
  public EquipmentClass newEquipmentClass;

  public enum EquipmentClass {
    Shield, Metal, Robe, Leather, Unarmed
  }

  public static Armour defaultArmour {
    get { return new Armour(EquipmentClass.Unarmed, 1); }
  }

  public override Equipment getDefault() { return defaultArmour; }

  public override Equipment upgrade(Equipment e1, Equipment e2) {
    return new Armour(equipmentClass, tier);
  }

  public Armour(EquipmentClass equipmentClass, int tier) {
    this.equipmentClass = equipmentClass;
    this.tier = tier;
    this.type = EquipType.armour;
  }

  public override string name() {
    return tier.ToString() + " " + equipmentClass;
  }
}

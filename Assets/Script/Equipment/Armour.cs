using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;

[System.Serializable]
public class Armour : Equipment {
  [FormerlySerializedAs("newEquipmentClass")]
  public EquipmentClass equipmentClass;

  public enum EquipmentClass {
    Shield, Metal, Robe, Leather, Unarmed
  }

  public static Armour defaultArmour {
    get; private set;
  }

  public override Equipment getDefault() { return defaultArmour; }

  public Armour(ArmourData armourData) {
    this.equipmentClass = armourData.equipmentClass;
    this.tier = armourData.tier;
    this.type = EquipType.armour;

    // temp: take the first created armor as the default
    if (defaultArmour == null) {
      defaultArmour = this;
    }
  }

  public override string name() {
    return tier.ToString() + " " + equipmentClass;
  }
}

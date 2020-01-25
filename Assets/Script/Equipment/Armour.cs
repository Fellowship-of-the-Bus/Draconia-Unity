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
    get { return new Armour(EquipmentDB.get.armour[0]); }
  }

  public override Equipment getDefault() { return defaultArmour; }

  public Armour(ArmourData armourData) {
    this.equipmentClass = armourData.equipmentClass;
    this.tier = armourData.tier;
    this.type = EquipType.armour;

    this.itemData = armourData;
  }

  public override string name() {
    return tier.ToString() + " " + equipmentClass;
  }
}

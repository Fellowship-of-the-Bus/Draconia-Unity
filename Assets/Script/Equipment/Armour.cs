using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Armour : Equipment {
  public enum ArmourKinds { Metal, Robe, Leather };
  public ArmourKinds kind = ArmourKinds.Metal;

  public override void upgrade() {

  }
  public Armour() {
    type = EquipType.armour;
  }

  public Armour(string equipmentClass, ArmourKinds kind, int tier) : this() {
    this.equipmentClass = equipmentClass;
    this.kind = kind;
    this.tier = tier;
  }
}

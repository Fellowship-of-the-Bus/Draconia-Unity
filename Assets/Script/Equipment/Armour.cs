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
}

using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Weapon : Equipment {
  public int range = 1;
  public enum kinds { Sharp, Blunt, Ranged };
  public kinds kind = kinds.Blunt;

  public override void upgrade() {

  }

  public Weapon() {
    type = EquipType.weapon;
  }

  public Weapon(string equipmentClass, kinds kind, int range, int tier) : this() {
    this.equipmentClass = equipmentClass;
    this.kind = kind;
    this.range = range;
    this.tier = tier;
  }

  /*public Weapon(kinds k) {
    range = k == Ranged ? 3 : 1;
    kind = k;
  }*/
}

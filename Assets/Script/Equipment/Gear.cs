using System;
using System.Collections.Generic;
using System.Runtime.Serialization; // for serialization callback
using UnityEngine;

[System.Serializable]
public class Gear {
  public Weapon weapon;
  public Armour armour;

  public Equipment this[int i] {
    get {
      if (i == EquipType.weapon) return weapon;
      if (i == EquipType.armour) return armour;
      return null;
    }
    set {
      if (i == EquipType.weapon) weapon = value as Weapon;
      if (i == EquipType.armour) armour = value as Armour;
    }
  }

  public Gear(Weapon w, Armour a) {
    weapon = w;
    armour = a;
  }

  public IEnumerator<Equipment> GetEnumerator()
  { //Needs to return in same order as EquipType orders the corresponding values
      yield return weapon;
      yield return armour;
  }
}

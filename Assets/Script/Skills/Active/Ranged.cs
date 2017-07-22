using UnityEngine;
using System.Collections.Generic;

public class Ranged: SingleTarget {
  public Ranged() {
    requireWeapon(Weapon.Kinds.Ranged);
    useWepRange = true;
    useLos = true;
    name = "Ranged";
    maxCooldown = 2;
  }

  public override int damageFormula() {
    return (int)(self.strength*(1+level*0.1));
  }
}

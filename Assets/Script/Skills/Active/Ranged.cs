using UnityEngine;
using System.Collections.Generic;

public class Ranged: SingleTarget {
  public Ranged() {
    requireWeapon(Weapon.Kinds.Ranged);
    useWepRange = true;
    useLos = true;
    name = "Ranged";
    maxCooldown = 2;
    dType = DamageType.physical;
  }

  public override int damageFormula() {
    return (int)(attributes.strength*(1+level*0.1));
  }
}

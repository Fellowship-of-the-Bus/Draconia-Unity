using UnityEngine;
using System.Collections.Generic;

public class ArcShot: SingleTarget {
  public ArcShot() {
    useWepRange = true;
    requireWeapon(Weapon.kinds.Ranged);
    useLos = false;
    name = "Arc Shot";
    cooldown = 2;
  }

  public override int calculateDamage(Character source, Character target) {
    return (int)(source.strength*(1+level*0.1) - target.physicalDefense);
  }
}

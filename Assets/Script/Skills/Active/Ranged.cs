using UnityEngine;
using System.Collections.Generic;

public class Ranged: SingleTarget {
  public Ranged() {
    requireWeapon(Weapon.kinds.Ranged);
    useWepRange = true;
    useLos = true;
    name = "Ranged";
    cooldown = 2;
  }

  public override int calculateDamage(Character source, Character target) {
    return (int)(source.strength*(1+level*0.1) - target.physicalDefense);
  }
}

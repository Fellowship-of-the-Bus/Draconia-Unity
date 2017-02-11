using UnityEngine;
using System.Collections.Generic;

public class FlameShot: SingleTarget {
  public FlameShot() {
    requireWeapon(Weapon.kinds.Ranged);
    useWepRange = true;
    useLos = true;
    name = "Flame Shot";
    maxCooldown = 2;
  }

  public override int calculateDamage(Character source, Character target) {
    return (int)((source.strength*(0.5+level*0.5) - target.physicalDefense)*target.fireResMultiplier);
  }
}

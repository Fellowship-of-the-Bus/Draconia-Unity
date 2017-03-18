using UnityEngine;
using System.Collections.Generic;
using System;

public class BluntShot: SingleTarget {
  public BluntShot() {
    requireWeapon(Weapon.kinds.Ranged);
    useWepRange = true;
    useLos = true;
    name = "Blunt Shot";
    maxCooldown = 2;
  }

  public override int damageFormula() {
    return (int)(self.strength*(0.5+level*0.05));
  }

  public override void additionalEffects (Character target) {
    target.curAction = Math.Max(0, target.curAction - 200);
    ActionQueue.get.updateTime(target.gameObject);
  }
}

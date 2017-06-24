﻿using UnityEngine;
using System.Collections.Generic;

public class ArcShot: SingleTarget {
  public ArcShot() {
    useWepRange = true;
    requireWeapon(Weapon.kinds.Ranged);
    useLos = false;
    name = "Arc Shot";
    maxCooldown = 2;
    targetAlly(false);
    targetEnemy(true);
  }

  public override int damageFormula() {
    return (int)(self.strength*(1+level*0.1));
  }
}

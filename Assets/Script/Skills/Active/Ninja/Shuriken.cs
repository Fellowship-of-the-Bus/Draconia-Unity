using UnityEngine;
using System.Collections.Generic;

public class Shuriken: SingleTarget {
  public Shuriken() {
    range = 3;
    useLos = true;
    name = "Shuriken";
    maxCooldown = 2;
    targetAlly(false);
    targetEnemy(true);
  }

  public override int damageFormula() {
    return (int)(self.strength*(1+level*0.1));
  }
}

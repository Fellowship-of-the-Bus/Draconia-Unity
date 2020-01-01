using UnityEngine;
using System.Collections.Generic;

public class Shuriken: SingleTarget {
  public Shuriken() {
    range = 3;
    useLos = true;
    name = "Shuriken";
    maxCooldown = 2;
    dType = DamageType.physical;

    targetAlly(false);
    targetEnemy(true);
  }

  public override int damageFormula() {
    return (int)(attributes.strength*(1+level*0.1));
  }

  protected override string tooltipDescription { get {
    return "Throw a shuriken which deals " + tooltipDamage + " to the target";
  }}
}

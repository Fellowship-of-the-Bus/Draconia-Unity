using UnityEngine;
using System.Collections.Generic;

public class Punch: SingleTarget {
  public Punch() {
    requireMelee();
    useWepRange = true;
    useLos = false;
    name = "Punch";
    maxCooldown = 1;
    dType = DamageType.physical;
    strAligned();

    targetAlly(false);
    targetEnemy(true);
  }

  public override string tooltipDescription { get {
    return "Deal " + tooltipDamage + " damage to the target";
  }}

  public override int damageFormula() {
    return (int)(attributes.strength*(1.4+level*0.1));
  }
}

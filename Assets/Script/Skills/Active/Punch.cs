using UnityEngine;
using System.Collections.Generic;

public class Punch: SingleTarget {
  public Punch() {
    requireMelee();
    useWepRange = true;
    useLos = false;
    name = "Punch";
    maxCooldown = 2;
    targetAlly(false);
    targetEnemy(true);
  }

  public override string tooltip { get { return "Range: Melee\n"
    + "Deal " + tooltipDamage + " damage to the target"; }}

  public override int damageFormula() {
    return (int)(self.strength*(1+level*0.1));
  }
}

using UnityEngine;
using System.Collections.Generic;

public class Incinerate: SingleTarget {
  public Incinerate() {
    range = 3;
    useWepRange = false;
    useLos = false;
    name = "Incinerate";
    maxCooldown = 2;

    dType = DamageType.magical;
    dEle = DamageElement.fire;
    targetAlly(false);
    targetEnemy(true);
  }

  protected override string tooltipDescription { get {
    return "Envelop the target in fire dealing " + tooltipDamage + " damage."
    + " Does not require line of sight.";
  }}

  public override int damageFormula() {
    return (int)(self.intelligence*(1.5f+level*0.1));
  }
}

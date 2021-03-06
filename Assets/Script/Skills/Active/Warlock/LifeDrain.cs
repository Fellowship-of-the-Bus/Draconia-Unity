using UnityEngine;
using System.Collections.Generic;

public class LifeDrain: SingleTarget {

  public override string animation { get { return "Cast"; }}
  public override Color castColor { get { return castColorWarlock; }}

  public LifeDrain() {
    range = 3;
    useWepRange = false;
    useLos = false;
    name = "Life Drain";
    maxCooldown = 2;

    dType = DamageType.magical;
    targetAlly(false);
    targetEnemy(true);
  }

  public override string tooltipDescription { get {
    return "Drain " + tooltipDamage + " from the target and heal for the amount drained";
  }}

  public override int damageFormula() {
    return (int)(attributes.intelligence*(1+level*0.1));
  }

  public override void additionalEffects(BattleCharacter target) {
    self.takeHealing(calculateDamage(target));
  }
}

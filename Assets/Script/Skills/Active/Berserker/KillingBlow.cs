using UnityEngine;
using System.Collections.Generic;

public class KillingBlow: SingleTarget {
  public KillingBlow() {
    requireMelee();
    useWepRange = true;
    useLos = false;
    name = "Killing Blow";
    maxCooldown = 2;
    dType = DamageType.physical;

    targetAlly(false);
    targetEnemy(true);
  }

  protected override string tooltipDescription { get { return
    "Attempt a finishing blow, dealing " + tooltipDamage
    + " to <color=" + tooltipDamageColor + ">" + (damageFormula() * 2)
    + "</color> damage based on the target's missing health"; }}

  public override int damageFormula() {
    return (int)(attributes.strength*(1+level*0.1));
  }

  public override int calculateDamage(BattleCharacter target, Tile attackOrigin = null) {
    float missingPct = 1 - (float)target.curHealth/target.maxHealth;
    return (int)(base.calculateDamage(target, attackOrigin)*(1 + missingPct * bonusFormula()));
  }

  private float bonusFormula() {
    return 0.5f + (0.1f * level);
  }
}

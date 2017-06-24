using UnityEngine;
using System.Collections.Generic;

public class KillingBlow: SingleTarget {
  public KillingBlow() {
    requireMelee();
    useWepRange = true;
    useLos = false;
    name = "Killing Blow";
    maxCooldown = 2;
    targetAlly(false);
    targetEnemy(true);
  }
  public override int damageFormula() {
    return (int)(self.strength*(1+level*0.1));
  }

  public override int calculateDamage(BattleCharacter target) {
    float missingPct = 1 - (float)target.curHealth/target.maxHealth;
    return (int)(base.calculateDamage(target)*(1 + missingPct * (0.5f + 0.1f * level)));
  }


}

using UnityEngine;
using System.Collections.Generic;

public class KillingBlow: SingleTarget {
  public KillingBlow() {
    requireMelee();
    useWepRange = true;
    useLos = false;
    name = "Killing Blow";
    cooldown = 2;
  }

  public override int calculateDamage(Character source, Character target) {
    float missingPct = 1 - (float)target.curHealth/target.maxHealth;

    return (int)((source.strength*(1+level*0.1) - target.physicalDefense) * (1 + missingPct * (0.5f + 0.1f * level)));
  }


}

using UnityEngine;
using System.Collections.Generic;

public class KillingBlow: SingleTarget {
  public KillingBlow() {
    range = 1;
    useWepRange = false;
    useLos = false;
    name = "Killing Blow";
  }

  public override int calculateDamage(Character source, Character target) {
    float missingPct = 1 - (float)target.curHealth/target.attr.maxHealth;

    return (int)((source.attr.strength*(1+level*0.1) - target.attr.physicalDefense) * (1 + missingPct * (0.5f + 0.1f * level)));
  }


}

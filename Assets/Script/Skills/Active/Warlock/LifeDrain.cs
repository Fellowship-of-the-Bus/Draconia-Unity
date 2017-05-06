using UnityEngine;
using System.Collections.Generic;

public class LifeDrain: SingleTarget {
  public LifeDrain() {
    range = 3;
    useWepRange = false;
    useLos = false;
    name = "Life Drain";
    maxCooldown = 2;


    dType = DamageType.magical;
  }

  public override int damageFormula() {
    return (int)(self.intelligence*(1+level*0.1));
  }

  public override void additionalEffects(BattleCharacter target) {
    self.takeHealing(calculateDamage(target));
  }
}

using UnityEngine;
using System.Collections.Generic;

public class Puncture: SingleTarget {
  public Puncture() {
    requireMelee();
    useWepRange = true;
    useLos = false;
    name = "Puncture";
    maxCooldown = 2;
    targetAlly(false);
    targetEnemy(true);
  }

  public override int damageFormula() {
    return (int)(self.strength*(0.5+level*0.05));
  }

  public override void additionalEffects (BattleCharacter target) {
    BleedEffect debuff = new BleedEffect();
    debuff.level = level;
    debuff.duration = (level+5)/2;
    debuff.damage = (int)System.Math.Max((int)calculateDamage(target)*(0.2f + 0.1f*level), 1);
    target.applyEffect(debuff);
  }
}

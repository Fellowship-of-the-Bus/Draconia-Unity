using UnityEngine;
using System.Collections.Generic;

public class Puncture: SingleTarget {
  public Puncture() {
    requireMelee();
    useWepRange = true;
    useLos = false;
    name = "Puncture";
    maxCooldown = 2;
    dType = DamageType.physical;

    targetAlly(false);
    targetEnemy(true);
  }

  public override int damageFormula() {
    return (int)(self.strength*(0.5+level*0.05));
  }

  public override void additionalEffects (BattleCharacter target) {
    BleedEffect debuff = new BleedEffect();
    debuff.duration = (level+5)/2;
    debuff.effectValue = (int)System.Math.Max((int)calculateDamage(target)*(0.2f + 0.1f*level), 1);
    debuff.caster = self;
    target.applyEffect(debuff);
  }

  protected override string tooltipDescription { get {
    return "Strike the target for " + tooltipDamage + " and inflict a bleed effect,"
    + " dealing X damage on each of their turns for Y turns";
  }}
}

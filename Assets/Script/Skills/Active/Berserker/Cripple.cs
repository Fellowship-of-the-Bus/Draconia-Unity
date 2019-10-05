using UnityEngine;
using System.Collections.Generic;

public class Cripple: SingleTarget {
  public Cripple() {
    requireMelee();
    useWepRange = true;
    useLos = false;
    name = "Cripple";
    maxCooldown = 2;
    dType = DamageType.physical;

    targetAlly(false);
    targetEnemy(true);
  }

  protected override string tooltipDescription { get {
    return "Deal " + tooltipDamage + " damage and cripple the target, reducing their movement range by 2";
  }}

  public override int damageFormula() {
    return (int)(attributes.strength*(1+level*0.05));
  }

  public override void additionalEffects (BattleCharacter target) {
    CrippleEffect debuff = new CrippleEffect();
    debuff.effectValue = 2; //maybe scale based off of level
    debuff.duration = 2;
    debuff.caster = self;
    target.applyEffect(debuff);
  }
}

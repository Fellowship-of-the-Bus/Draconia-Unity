using UnityEngine;
using System.Collections.Generic;

public class PoisonShot: SingleTarget {
  public override string tooltipDescription { get {
    return "Fire a poisonous arrow that deals " + tooltipDamage + " and poisons the target for "
      + effectDuration() + " turns";
  }}

  public override string animation { get { return "Shoot"; }}

  public PoisonShot() {
    requireWeapon(Weapon.Kind.Ranged);
    useWepRange = true;
    useLos = true;
    name = "Poison Shot";
    maxCooldown = 2;
    dType = DamageType.physical;

    targetAlly(false);
    targetEnemy(true);
  }

  public override int damageFormula() {
    return (int)(attributes.strength*(0.5+level*0.05));
  }

  int effectDuration() {
    return (level+5)/2;
  }

  public override void additionalEffects (BattleCharacter target) {
    PoisonEffect debuff = new PoisonEffect();
    debuff.duration = effectDuration();
    debuff.effectValue = (int)System.Math.Max((int)calculateDamage(target)*(0.2f + 0.1f*level), 1);
    debuff.caster = self;
    target.applyEffect(debuff);
  }
}

using UnityEngine;
using System.Collections.Generic;

public class SerratedShot: SingleTarget {
  protected override string tooltipDescription { get {
    return "Fire a serrated arrow that deals " + tooltipDamage + " and causes the target to bleed dealing an additional "
    + formatDamage(effectDamageFormula()) + " over " + bleedDuration() + " turns";
  }}

  public override string animation { get { return "Shoot"; }}

  public SerratedShot() {
    requireWeapon(Weapon.Kinds.Ranged);
    useWepRange = true;
    useLos = true;
    name = "Serrated Shot";
    maxCooldown = 2;
    dType = DamageType.physical;

    targetAlly(false);
    targetEnemy(true);
  }

  public override int damageFormula() {
    return (int)(attributes.strength*(0.5+level*0.05));
  }

  int effectDamageFormula() {
    return (int)(damageFormula() * (0.2f + 0.1f*level));
  }

  int bleedDuration() {
    return (level+5)/2;
  }

  public override void additionalEffects (BattleCharacter target) {
    BleedEffect debuff = new BleedEffect();
    debuff.duration = (level+5)/2;
    debuff.effectValue = (int)System.Math.Max(effectDamageFormula(), 1);
    debuff.caster = self;
    target.applyEffect(debuff);
  }
}

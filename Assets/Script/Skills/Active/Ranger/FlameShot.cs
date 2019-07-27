using UnityEngine;
using System.Collections.Generic;

public class FlameShot: SingleTarget {
  const int fireDuration = 3;
  public override string animation { get { return "Shoot"; }}

  public FlameShot() {
    requireWeapon(Weapon.Kinds.Ranged);
    useWepRange = true;
    useLos = true;
    name = "Flame Shot";
    maxCooldown = 2;

    dEle = DamageElement.fire;
    targetAlly(false);
    targetEnemy(true);
    projectileType = ProjectileType.FlameArrow;
    projectileMoveType = ProjectileMovementType.Straight;
    projectileSpeed = 1.5f;
  }

  protected override string tooltipDescription { get {
    return "Fire a flaming arrow that deals " + tooltipDamage + " and sets the target aflame, dealing "
    + formatDamage(effectDamageFormula()) + " on their next turn and decreasing over " + fireDuration + " turns.\n" +
      "Total damage of fire effect: " + formatDamage(BurnEffect.totalDamage(effectDamageFormula(), fireDuration));
  }}

  public override int damageFormula() {
    return (int)(self.strength*(0.5+level*0.5));
  }

  int effectDamageFormula() {
    return (int)(damageFormula() * (0.2f + 0.1f*level));
  }

  public override void additionalEffects (BattleCharacter target) {
    BurnEffect debuff = new BurnEffect();
    debuff.duration = fireDuration;
    debuff.effectValue = (int)System.Math.Max((int)effectDamageFormula(), 1);
    debuff.caster = self;
    target.applyEffect(debuff);
  }
}

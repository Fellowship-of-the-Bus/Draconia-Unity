using UnityEngine;
using System.Collections.Generic;

public class FlameShot: SingleTarget {

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

  public override int damageFormula() {
    return (int)(self.strength*(0.5+level*0.5));
  }

  public override void additionalEffects (BattleCharacter target) {
    BurnEffect debuff = new BurnEffect();
    debuff.level = level;
    debuff.duration = (level+5)/2;
    debuff.damage = (int)System.Math.Max((int)calculateDamage(target)*(0.2f + 0.1f*level), 1);
    target.applyEffect(debuff);
  }
}

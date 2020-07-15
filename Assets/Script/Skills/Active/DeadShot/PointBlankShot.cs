using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PointBlankShot: SingleTarget {
  float rangeMultiplier = 2.5f;

  public override string animation { get { return "Shoot"; }}

  public PointBlankShot() {
    requireWeapon(Weapon.Kind.Ranged);
    useWepRange = true;
    useLos = true;
    name = "Point Blank Shot";
    maxCooldown = 2;
    dType = DamageType.physical;

    targetAlly(false);
    targetEnemy(true);
    projectileType = ProjectileType.Arrow;
    projectileMoveType = ProjectileMovementType.Straight;
  }
  public override int damageFormula() {
    return (int)(attributes.strength*(1+level*0.1));
  }

  public override string tooltipDescription { get {
    return "Deal up to <color=" + tooltipDamageColor + ">" + (int)(damageFormula() * rangeMultiplier)
    + "</color> damage, decreasing against targets further away.";
  }}

  public override int calculateDamage(BattleCharacter target, Tile attackOrigin = null) {
    float distance = (target.transform.position - self.transform.position).magnitude;
    float multiplier = rangeMultiplier / distance;

    return (int)(base.calculateDamage(target, attackOrigin) * multiplier);
  }
}

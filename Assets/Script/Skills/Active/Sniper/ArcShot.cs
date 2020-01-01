using UnityEngine;
using System.Collections.Generic;

public class ArcShot: SingleTarget {

  public override string animation { get { return "Shoot"; }}

  public ArcShot() {
    useWepRange = true;
    requireWeapon(Weapon.Kind.Ranged);
    useLos = false;
    name = "Arc Shot";
    maxCooldown = 2;
    dType = DamageType.physical;
    strAligned();

    targetAlly(false);
    targetEnemy(true);
    projectileType = ProjectileType.Arrow;
    projectileMoveType = ProjectileMovementType.Parabolic;
  }

  protected override string tooltipDescription { get {
    return "Fire an arcing shot that ignores line of sight and deals " + tooltipDamage + " damage to the target";
  }}

  public override int damageFormula() {
    return (int)(self.strength*(1.4+level*0.1));
  }
}

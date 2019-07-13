using UnityEngine;
using System.Collections.Generic;

public class ArcShot: SingleTarget {

  public override string animation { get { return "Shoot"; }}

  public ArcShot() {
    useWepRange = true;
    requireWeapon(Weapon.Kinds.Ranged);
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

  public override int damageFormula() {
    return (int)(self.strength*(1.4+level*0.1));
  }
}

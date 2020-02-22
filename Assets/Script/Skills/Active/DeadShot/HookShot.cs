using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class HookShot: TargetMover {

  public override string animation { get { return "Shoot"; }}

  public HookShot() {
    requireWeapon(Weapon.Kind.Ranged);
    useWepRange = true;
    useLos = true;
    name = "Hook Shot";
    maxCooldown = 2;
    dType = DamageType.physical;

    targetAlly(false);
    targetEnemy(true);
    setDirection(TargetMover.Direction.towards);
  }

  public override string tooltipDescription { get {
    return "Deal " + tooltipDamage + " damage and pull the target up to 2 spaces closer";
  }}

  public override int damageFormula() {
    return (int)(attributes.strength*(1+level*0.1));
  }
}

using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ForceShot: TargetMover {

  public override string animation { get { return "Shoot"; }}

  public ForceShot() {
    requireWeapon(Weapon.Kinds.Ranged);
    useWepRange = true;
    useLos = true;
    name = "Force Shot";
    maxCooldown = 2;
    targetAlly(false);
    targetEnemy(true);
    setDirection(TargetMover.Direction.away);
  }

  protected override string tooltipDescription { get {
    return "Deal " + tooltipDamage + " damage and knock the target back 2 spaces";
  }}

  public override int damageFormula() {
    return (int)(self.strength*(1+level*0.1));
  }
}

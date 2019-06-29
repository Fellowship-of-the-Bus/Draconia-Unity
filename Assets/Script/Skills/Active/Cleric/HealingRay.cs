using UnityEngine;
using System.Collections.Generic;

public class HealingRay: SingleTarget, HealingSkill {

  public override string animation { get { return "ClericCast"; }}
  public override Color castColor { get { return castColorCleric; }}

  public HealingRay() {
    range = 3;
    useWepRange = false;
    useLos = false;
    name = "Healing Ray";
    maxCooldown = 2;
    canTargetSelf = true;
    targetAlly(true);
    targetEnemy(false);

    projectileType = ProjectileType.HealingRay;
    projectileMoveType = ProjectileMovementType.Laser;
    projectileSpeed = 3f;
  }

  protected override string tooltipDescription { get {
    return "Heal the target by " + tooltipHealing;
  }}

  public int healingFormula() {
    return (int)(self.intelligence*(1+level*0.1));
  }
}

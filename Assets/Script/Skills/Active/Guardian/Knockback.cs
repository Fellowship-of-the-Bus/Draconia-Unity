using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Knockback: TargetMover {
  public Knockback() {
    requireMelee();
    useWepRange = true;
    useLos = false;
    name = "Knockback";
    maxCooldown = 2;
    targetAlly(false);
    targetEnemy(true);
    setDirection(TargetMover.Direction.away);
    setDistance(2);
  }

  protected override string tooltipDescription { get {
    return "Deal " + tooltipDamage + " damage and knock the target back 2 spaces";
  }}
  
  float upThreshold = 0.5f;

  public override int damageFormula() {
    return (int)(self.strength*(1+level*0.1));
  }
}

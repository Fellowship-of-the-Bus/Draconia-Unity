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

  public override string tooltip { get { return "Deal " + damageFormula().ToString() + " damage and knock the target back"; }}
  float upThreshold = 0.5f;

  public override int damageFormula() {
    return (int)(self.strength*(1+level*0.1));
  }
}

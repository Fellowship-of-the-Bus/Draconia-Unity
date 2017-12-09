using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ForceShot: TargetMover {

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

  public override int damageFormula() {
    return (int)(self.strength*(1+level*0.1));
  }
}

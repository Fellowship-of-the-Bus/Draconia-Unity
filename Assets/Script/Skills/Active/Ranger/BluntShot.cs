using UnityEngine;
using System.Collections.Generic;
using System;

public class BluntShot: SingleTarget {

  public override string animation { get { return "Shoot"; }}

  public BluntShot() {
    requireWeapon(Weapon.Kinds.Ranged);
    useWepRange = true;
    useLos = true;
    name = "Blunt Shot";
    maxCooldown = 2;
    targetAlly(false);
    targetEnemy(true);
  }

  public override int damageFormula() {
    return (int)(self.strength*(0.5+level*0.05));
  }

  public override void additionalEffects (BattleCharacter target) {
    target.curAction = Math.Max(0, target.curAction - 200);
    ActionQueue.get.updateTime(target.gameObject);
  }
}

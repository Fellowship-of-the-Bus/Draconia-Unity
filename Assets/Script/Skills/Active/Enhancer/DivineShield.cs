using UnityEngine;
using System.Collections.Generic;

public class DivineShield: SingleTarget {

  public override string animation { get { return "ClericCast"; }}

  public DivineShield() {
    range = 5;
    useLos = false;
    name = "DivineShield";
    maxCooldown = 2;
    canTargetSelf = true;
    targetAlly(true);
    targetEnemy(false);
  }

  public override void additionalEffects (BattleCharacter target) {
    DodgeEffect e = new DodgeEffect();
    e.level = level;
    e.duration = 3;
    target.applyEffect(e);
  }
}

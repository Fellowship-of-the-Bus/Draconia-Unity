using UnityEngine;
using System.Collections.Generic;

public class DivineShield: SingleTarget {

  public override string animation { get { return "ClericCast"; }}
  public override Color castColor { get { return castColorEnhancer; }}

  public DivineShield() {
    range = 5;
    useLos = false;
    name = "Divine Shield";
    maxCooldown = 2;
    canTargetSelf = true;
    targetAlly(true);
    targetEnemy(false);
  }

  public override void additionalEffects (BattleCharacter target) {
    DodgeEffect e = new DodgeEffect();
    e.duration = 3;
    e.caster = self;
    target.applyEffect(e);
  }
}

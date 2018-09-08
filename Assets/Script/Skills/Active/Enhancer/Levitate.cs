using UnityEngine;
using System.Collections.Generic;
using System;

public class Levitate: SingleTarget {

  public override string animation { get { return "ClericCast"; }}
  public override Color castColor { get { return castColorEnhancer; }}

  public Levitate() {
    range = 3;
    useWepRange = false;
    useLos = false;
    name = "Levitate";
    maxCooldown = 2;
    canTargetSelf = true;
    targetAlly(true);
    targetEnemy(false);
  }

  public override void additionalEffects (BattleCharacter target) {
    LevitateEffect debuff = new LevitateEffect();
    debuff.level = level;
    debuff.duration = 3;
    target.applyEffect(debuff);
  }

}

using UnityEngine;
using System.Collections.Generic;
using System;

public class Levitate: SingleTarget {
  public Levitate() {
    range = 3;
    useWepRange = false;
    useLos = false;
    name = "Levitate";
    maxCooldown = 2;
  }

  public override void additionalEffects (BattleCharacter target) {
    LevitateEffect debuff = new LevitateEffect();
    debuff.level = level;
    debuff.duration = 3;
    target.applyEffect(debuff);
  }

}

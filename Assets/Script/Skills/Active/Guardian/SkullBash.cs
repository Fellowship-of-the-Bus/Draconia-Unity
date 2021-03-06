﻿using UnityEngine;
using System.Collections.Generic;
using System;

public class SkullBash: SingleTarget {
  public SkullBash() {
    range = 1;
    useWepRange = false;
    useLos = false;
    name = "Skull Bash";
    maxCooldown = 2;
    dType = DamageType.physical;

    targetAlly(false);
    targetEnemy(true);
  }

  public override string tooltipDescription { get {
    return "Unleash a dizzying blow dealing " + tooltipDamage + " damage to the target, "
    + "decreasing their speed for 3 turns and delaying their next turn";
  }}

  public override int damageFormula() {
    return (int)(attributes.strength*(0.5+level*0.05));
  }

  public override void additionalEffects (BattleCharacter target) {
    SlowEffect debuff = new SlowEffect();
    target.curAction = Math.Max(0, target.curAction - 300);
    debuff.effectValue = level;
    debuff.duration = 3;
    debuff.caster = self;
    target.applyEffect(debuff);
    ActionBar.get.updateTime(target);
  }
}

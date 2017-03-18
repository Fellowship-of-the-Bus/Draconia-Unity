using UnityEngine;
using System.Collections.Generic;
using System;

public class SkullBash: SingleTarget {
  public SkullBash() {
    range = 1;
    useWepRange = false;
    useLos = false;
    name = "Skull Bash";
    maxCooldown = 2;
  }

  public override int damageFormula() {
    return (int)(self.strength*(0.5+level*0.05));
  }

  public override void additionalEffects (Character target) {
    SlowEffect debuff = new SlowEffect();
    target.curAction = Math.Max(0, target.curAction - 300);
    debuff.level = level;
    debuff.duration = 3;
    target.applyEffect(debuff);
    ActionQueue.get.updateTime(target.gameObject);
  }

}

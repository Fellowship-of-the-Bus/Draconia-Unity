using UnityEngine;
using System.Collections.Generic;
using System;

public class SkullBash: SingleTarget {
  public SkullBash() {
    range = 1;
    useWepRange = false;
    useLos = false;
    name = "Skull Bash";
    cooldown = 2;
  }

  public override int calculateDamage(Character source, Character target) {
    return (int)(source.attr.strength*(0.5+level*0.05) - target.attr.physicalDefense);
  }

  public override void additionalEffects (Character target) {
    DazedEffect debuff = new DazedEffect();
    target.curAction = Math.Max(0, target.curAction - 300);
    debuff.level = level;
    debuff.duration = 3;
    target.applyEffect(debuff);
    ActionQueue.get.updateTime(target.gameObject);
  }

}

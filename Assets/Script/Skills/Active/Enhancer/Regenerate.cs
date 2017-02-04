using UnityEngine;
using System.Collections.Generic;

public class Regenerate: SingleTarget, HealingSkill {
  public Regenerate() {
    range = 3;
    useWepRange = false;
    useLos = false;
    name = "Regenerate";
    cooldown = 2;
  }

  public int calculateHealing(Character source, Character target) {
    return 0;
  }

  public override void additionalEffects(Character target) {
    RegenerationEffect buff = new RegenerationEffect();
    buff.level = level;
    buff.duration = (level+5)/2;
    buff.healing = (int)System.Math.Max((int)calculateDamage(self, target)*(0.2f + 0.1f*level), 1);
    target.applyEffect(buff);
  }
}

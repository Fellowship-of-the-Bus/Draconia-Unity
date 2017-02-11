using UnityEngine;
using System.Collections.Generic;

public class Transfusion: SingleTarget, HealingSkill {
  public Transfusion() {
    range = 3;
    useWepRange = false;
    useLos = false;
    name = "Transfusion";
    maxCooldown = 0;
  }

  public int calculateHealing(Character source, Character target) {
    return (int)(source.intelligence*(1+level*0.1) * target.healingMultiplier);
  }

  public override void additionalEffects(Character target) {
    self.takeDamage((int)(self.intelligence * (1 + level * 0.1)));
  }
}

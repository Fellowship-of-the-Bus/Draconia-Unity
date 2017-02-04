using UnityEngine;
using System.Collections.Generic;

public class HealingRay: SingleTarget, HealingSkill {
  public HealingRay() {
    range = 3;
    useWepRange = false;
    useLos = false;
    name = "Healing Ray";
    cooldown = 2;
  }

  public int calculateHealing(Character source, Character target) {
    return (int)(source.intelligence*(1+level*0.1) * target.healingMultiplier);
  }
}

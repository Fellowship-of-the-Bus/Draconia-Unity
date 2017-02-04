using UnityEngine;
using System.Collections.Generic;

public class HealingTouch: SingleTarget, HealingSkill {
  public HealingTouch() {
    range = 1;
    useWepRange = false;
    useLos = false;
    name = "Healing Touch";
    maxCooldown = 2;
  }

  public int calculateHealing(Character source, Character target) {
    return (int)(source.intelligence*(1+level*0.5) * target.healingMultiplier);
  }
}

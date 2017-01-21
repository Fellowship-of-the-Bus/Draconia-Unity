using UnityEngine;
using System.Collections.Generic;

public class Heal: SingleTarget, HealingSkill {
  public Heal() {
    range = 3;
    useWepRange = false;
    useLos = false;
    name = "Heal";
    cooldown = 2;
  }

  public int calculateHealing(Character source, Character target) {
    return (int)(source.attr.intelligence*(1+level*0.1) * target.attr.healingMultiplier);
  }
}

using UnityEngine;
using System.Collections.Generic;

public class HealingCircle: CircleAoE, HealingSkill {
  public HealingCircle() {
    range = 3;
    useWepRange = false;
    aoe = 2;
    useLos = false;
    name = "Healing Circle";
    effectsTiles = false;
    maxCooldown = 2;
  }

  public int healingFormula() {
    return (int)(self.intelligence*(1+level*0.1));
  }
}

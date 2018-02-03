using UnityEngine;
using System.Collections.Generic;

public class HealingTouch: SingleTarget, HealingSkill {
  public HealingTouch() {
    range = 1;
    useWepRange = false;
    useLos = false;
    name = "Healing Touch";
    maxCooldown = 2;
    canTargetSelf = true;
    targetAlly(true);
    targetEnemy(false);
  }

  protected override string tooltipDescription { get {
    return "Heal the target by " + tooltipHealing;
  }}

  public int healingFormula() {
    return (int)(self.intelligence*(1+level*0.5));
  }
}

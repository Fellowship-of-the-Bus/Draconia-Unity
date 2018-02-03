using UnityEngine;
using System.Collections.Generic;

public class HealingCircle: CircleAoE, HealingSkill {

  public override string animation { get { return "ClericCast"; }}

  public HealingCircle() {
    range = 3;
    useWepRange = false;
    aoe = 2;
    useLos = false;
    name = "Healing Circle";
    effectsTiles = false;
    maxCooldown = 2;
    targetAlly(true);
    targetEnemy(false);
  }

  protected override string tooltipDescription { get {
    return "Heal allies in the affected area by " + tooltipHealing;
  }}

  public int healingFormula() {
    return (int)(self.intelligence*(1+level*0.1));
  }
}

using UnityEngine;
using System.Collections.Generic;

public class HealingTouch: SingleTarget, HealingSkill {

  public override string animation { get { return "ClericCast"; }}
  public override Color castColor { get { return castColorCleric; }}

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
    return "Touch a target, healing them by " + tooltipHealing;
  }}

  public int healingFormula() {
    return (int)(attributes.intelligence*(1+level*0.5));
  }
}

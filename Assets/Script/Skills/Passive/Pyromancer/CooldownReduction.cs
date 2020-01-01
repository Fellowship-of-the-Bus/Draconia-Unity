using UnityEngine;
using System.Collections.Generic;
using System;

public class CooldownReduction : PassiveSkill {
  public CooldownReduction() {
    name = "Cooldown Reduction";
  }

  protected override string tooltipDescription { get {
    return "Reduce maximum cooldown of all skills by 1";
  }}

  protected override void onActivate() {
    foreach (ActiveSkill skill in self.equippedSkills) {
      skill.maxCooldown = (int)Math.Max(skill.maxCooldown - 1, 0);
    }
  }

  protected override void onDeactivate() {}
}

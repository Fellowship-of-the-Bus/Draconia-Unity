using UnityEngine;
using System.Collections.Generic;
using System;

public class CooldownReduction : PassiveSkill {
  protected override void onActivate() {
    foreach (ActiveSkill skill in self.equippedSkills) {
      skill.cooldown = (int)Math.Max(skill.cooldown - 1, 0);
    }
  }

  protected override void onDeactivate() {}
}

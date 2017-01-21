using UnityEngine;
using System.Collections.Generic;
using System;

public class CooldownReduction : PassiveSkill {

  public override List<GameObject> getTargets() {
    List<GameObject> targets = new List<GameObject>();
    targets.Add(self.gameObject);
    return targets;
  }

  public override void onActivate() {
    foreach (ActiveSkill skill in self.equippedSkills) {
      skill.cooldown = (int)Math.Max(skill.cooldown - 1, 0);
    }
  }

  public override void onDeactivate() {}
}

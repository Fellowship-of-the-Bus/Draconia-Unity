using UnityEngine;
using System.Collections.Generic;

public class BrainFreeze: SingleTarget {
  public BrainFreeze() {
    useWepRange = false;
    useLos = false;
    name = "BrainFreeze";
    maxCooldown = 2;
    range = 3;
  }

  public override int calculateDamage(Character source, Character target) {
    return 0;
  }

  public override void additionalEffects (Character target) {
    foreach (ActiveSkill a in target.equippedSkills) {
      if (a.curCooldown > 0) a.curCooldown += level;
    }
  }
}

using UnityEngine;
using System.Collections.Generic;

public class DivineShield: SingleTarget {
  public DivineShield() {
    range = 5;
    useLos = false;
    name = "DivineShield";
    maxCooldown = 2;
    canTargetSelf = true;
  }

  public override void additionalEffects (Character target) {
    DodgeEffect e = new DodgeEffect();
    e.level = level;
    e.duration = 3;
    target.applyEffect(e);
  }
}

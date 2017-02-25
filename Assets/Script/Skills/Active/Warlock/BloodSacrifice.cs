using UnityEngine;
using System.Collections.Generic;
using System;

public class BloodSacrifice : SelfTarget {
  public BloodSacrifice() {
    range = 0;
    useWepRange = false;
    useLos = false;
    name = "Blood Sacrifice";
    maxCooldown = 2;
  }

  public override void additionalEffects(Character target) {
    BloodSacrificeEffect e = new BloodSacrificeEffect();
    e.setLevel(level);
    e.duration = 2;
    target.applyEffect(e);
    target.takeDamage(2);
  }
}

using UnityEngine;
using System.Collections.Generic;
using System;

public class BloodSacrifice : ActiveSkill {

  public BloodSacrifice() {
    range = 0;
    useWepRange = false;
    useLos = false;
    name = "Blood Sacrifice";
    maxCooldown = 2;
  }
  public override List<GameObject> getTargets() {
    List<GameObject> targets = new List<GameObject>();
    targets.Add(self.gameObject);
    return targets;
  }

  public override void additionalEffects(Character target) {
    BloodSacrificeEffect e = new BloodSacrificeEffect();
    e.setLevel(1);
    e.duration = 2;
    target.applyEffect(e);
    target.takeDamage(2);
  }
}

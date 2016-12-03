using System;
using UnityEngine;
using System.Collections.Generic;

public class DodgeSkill : PassiveSkill {
  public override void activate(Character target) {
    DodgeEffect effect = new DodgeEffect();
    effect.level = level;
    target.applyEffect(effect);
  }

  public override List<GameObject> getTargets() {
    List<GameObject> targets = new List<GameObject>();
    targets.Add(self.gameObject);
    return targets;
  }
}

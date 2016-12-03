using System;
using System.Collections.Generic;
using UnityEngine;

public class CounterSkill : PassiveSkill {
  public override void activate(Character target) {
    CounterEffect effect = new CounterEffect();
    effect.level = level;
    target.applyEffect(effect);
  }

  public override List<GameObject> getTargets() {
    List<GameObject> targets = new List<GameObject>();
    targets.Add(self.gameObject);
    return targets;
  }
}

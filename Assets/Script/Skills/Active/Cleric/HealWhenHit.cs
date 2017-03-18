using UnityEngine;
using System.Collections.Generic;

public class HealWhenHit : CircleAoE {
  public HealWhenHit() {
    range = 3;
    useWepRange = false;
    aoe = 3;
    useLos = false;
    name = "HealWhenHit";
    effectsTiles = false;
    maxCooldown = 2;
  }

  public override void additionalEffects(Character target) {
    HealWhenHitEffect e = new HealWhenHitEffect();
    e.duration = 5;
    e.level = level;
    target.applyEffect(e);
  }
}

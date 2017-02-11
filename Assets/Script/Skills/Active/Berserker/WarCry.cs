using UnityEngine;
using System.Collections.Generic;

public class WarCry: CircleAoE {
  public WarCry() {
    range = 0;
    aoe = 3;
    useLos = false;
    name = "WarCry";
    effectsTiles = false;
    maxCooldown = 2;
  }

  public override void additionalEffects (Character target) {
    WarCryEffect e = new WarCryEffect();
    e.level = level;
    e.duration = 1;
    target.applyEffect(e);
  }

  public override int calculateDamage(Character source, Character target) {
    return 0;
  }
}

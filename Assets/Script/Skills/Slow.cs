using UnityEngine;
using System.Collections.Generic;

public class Slow: SingleTarget {
  public Slow() {
    useWepRange = false;
    range = 5;
    useLos = false;
    name = "Slow";
    maxCooldown = 2;
  }

  public override int calculateDamage(Character source, Character target) {
    return 0;
  }

  public override void additionalEffects (Character target) {
    SlowEffect debuff = new SlowEffect();
    debuff.level = level*2;
    debuff.duration = 2;
    target.applyEffect(debuff);
  }
}
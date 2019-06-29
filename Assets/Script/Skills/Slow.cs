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

  public override void additionalEffects (BattleCharacter target) {
    SlowEffect debuff = new SlowEffect();
    debuff.level = level*2;
    debuff.duration = 2;
    debuff.caster = self;
    target.applyEffect(debuff);
  }
}

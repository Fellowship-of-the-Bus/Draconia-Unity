using UnityEngine;
using System;
using System.Collections.Generic;

public class RegenerateAura: SelfTarget {
  public int aoe {get; set;}

  public RegenerateAura() {
    range = 0;
    useWepRange = false;
    aoe = 2;
    useLos = false;
    name = "RegenerateAura";
    maxCooldown = 2;
  }

  public override void additionalEffects (Character target) {
    Func<RegenerationEffect> f = () => {
      RegenerationEffect buff = new RegenerationEffect();
      buff.level = level;
      buff.healing = 2;
      buff.duration = -1;
      return buff;
    };

    Aura<RegenerationEffect> e = new Aura<RegenerationEffect>(aoe, f);
    e.level = level;
    e.duration = 2;
    e.applyToSelf = true;

    target.applyEffect(e);
  }
}

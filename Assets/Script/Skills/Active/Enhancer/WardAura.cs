using UnityEngine;
using System;
using System.Collections.Generic;

public class WardAura: SelfTarget {
  public int aoe {get; set;}

  public WardAura() {
    range = 0;
    useWepRange = false;
    aoe = 2;
    useLos = false;
    name = "WardAura";
    maxCooldown = 2;
  }

  public override void additionalEffects (Character target) {
    Func<WardEffect> f = () => {
      WardEffect eff = new WardEffect();
      eff.duration = -1;
      return eff;
    };

    Aura<WardEffect> e = new Aura<WardEffect>(aoe, f);
    e.level = level;
    e.duration = 2;
    e.applyToSelf = true;

    target.applyEffect(e);
  }
}

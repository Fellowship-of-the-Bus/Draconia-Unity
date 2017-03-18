using UnityEngine;
using System;
using System.Collections.Generic;

public class EnlightenAura: SelfTarget {
  public int aoe {get; set;}

  public EnlightenAura() {
    range = 0;
    useWepRange = false;
    aoe = 2;
    useLos = false;
    name = "EnlightenAura";
    maxCooldown = 2;
  }

  public override void additionalEffects (Character target) {
    Func<EnlightenEffect> f = () => {
      EnlightenEffect eff = new EnlightenEffect();
      eff.duration = -1;
      return eff;
    };

    Aura<EnlightenEffect> e = new Aura<EnlightenEffect>(aoe, f);
    e.level = level;
    e.duration = 2;
    e.applyToSelf = true;

    target.applyEffect(e);
  }

}

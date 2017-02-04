using UnityEngine;
using System;
using System.Collections.Generic;

public class SlowAura: PassiveSkill {
  public int aoe {get; set;}

  protected override void onApply(Character c) {
    range = 0;
    aoe = 2;
    useLos = false;
    name = "SlowAura";

    Func<SlowEffect> f = () => {
      SlowEffect eff = new SlowEffect();
      eff.duration = -1;
      return eff;
    };

    Aura<SlowEffect> e = new Aura<SlowEffect>(aoe, f);
    e.level = level;
    e.duration = -1;
    e.applyToSelf = true;

    owner.applyEffect(e);

  }

  protected override void onActivate() {
  }

  protected override void onDeactivate() {
  }
}

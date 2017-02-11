using UnityEngine;
using System;
using System.Collections.Generic;

public class HasteAura: PassiveSkill {
  public int aoe {get; set;}

  protected override void onApply(Character c) {
    range = 0;
    aoe = 2;
    useLos = false;
    name = "Haste Aura";

    Func<HasteEffect> f = () => {
      HasteEffect eff = new HasteEffect();
      eff.duration = -1;
      return eff;
    };

    Aura<HasteEffect> e = new Aura<HasteEffect>(aoe, f);
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

using UnityEngine;
using System;
using System.Collections.Generic;

public class ExsanguinationAura: PassiveSkill {
  public int aoe {get; set;}

  protected override void onApply(BattleCharacter c) {
    aoe = 3;
    useLos = false;
    name = "Exsanguination Aura";

    Func<BleedEffect> f = () => {
      BleedEffect eff = new BleedEffect();
      eff.duration = -1;
      return eff;
    };

    Aura<BleedEffect> e = new Aura<BleedEffect>(aoe, f, true);
    e.effectValue = owner.attr.intelligence;
    e.duration = -1;
    e.applyToSelf = true;

    e.caster = self;
    owner.applyEffect(e);
  }

  protected override void onActivate() {
  }

  protected override void onDeactivate() {
  }
}

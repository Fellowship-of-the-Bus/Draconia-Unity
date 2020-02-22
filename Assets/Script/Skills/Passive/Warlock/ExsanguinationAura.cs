using UnityEngine;
using System;
using System.Collections.Generic;

public class ExsanguinationAura: PassiveSkill {
  public int aoe {get; set;}

  public ExsanguinationAura() {
    name = "Exsanguination Aura";
    aoe = 3;
    useLos = false;
  }

  public override string tooltipDescription { get {
    return "Range: " + aoe.ToString() + "\n"
      + "Causes enemies to bleed while in range, taking " + effectDamageFormula().ToString() + " damage each turn";
  }}

  int effectDamageFormula() {
    return attributes.intelligence;
  }

  protected override void onApply(BattleCharacter c) {
    Func<BleedEffect> f = () => {
      BleedEffect eff = new BleedEffect();
      eff.duration = -1;
      return eff;
    };

    Aura<BleedEffect> e = new Aura<BleedEffect>(aoe, f, true);
    e.effectValue = effectDamageFormula();
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

using UnityEngine;
using System;
using System.Collections.Generic;

public class SlowAura: PassiveSkill {
  public int aoe {get; set;}

  public SlowAura() {
    name = "Slow Aura";
  }

  public override string tooltipDescription { get {
    // TODO: include the AoE and the amount of speed decrease
    return "Decrease the speed of nearby enemies";
  }}

  protected override void onApply(BattleCharacter c) {
    aoe = 2;
    useLos = false;
    name = "SlowAura";

    Func<SlowEffect> f = () => {
      SlowEffect eff = new SlowEffect();
      eff.duration = -1;
      return eff;
    };

    Aura<SlowEffect> e = new Aura<SlowEffect>(aoe, f, true);
    e.effectValue = level;
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

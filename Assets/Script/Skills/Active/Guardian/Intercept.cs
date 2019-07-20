using UnityEngine;
using System;
using System.Collections.Generic;

public class Intercept: SelfTarget {
  public int aoe {get; set;}

  public Intercept() {
    range = 0;
    useWepRange = false;
    aoe = 1;
    useLos = false;
    name = "Intercept";
    maxCooldown = 2;
    targetAlly(true);
    targetEnemy(false);
  }

  protected override string tooltipDescription { get {
    return "Intercept attacks aimed at adjacent allies, taking the hit in their place.";
  }}

  public override void additionalEffects (BattleCharacter target) {
    Func<InterceptEffect> f = () => {
      InterceptEffect eff = new InterceptEffect();
      eff.origin = self;
      eff.duration = -1;
      return eff;
    };

    Aura<InterceptEffect> e = new Aura<InterceptEffect>(aoe, f);
    e.duration = 2;
    e.applyToSelf = false;

    e.caster = self;
    target.applyEffect(e);
  }
}

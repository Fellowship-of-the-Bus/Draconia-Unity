using UnityEngine;
using System;
using System.Collections.Generic;

public class WardAura: SelfTarget {
  public int aoe {get; set;}

  public override string animation { get { return "ClericCast"; }}
  public override Color castColor { get { return castColorEnhancer; }}

  public WardAura() {
    range = 0;
    useWepRange = false;
    aoe = 2;
    useLos = false;
    name = "WardAura";
    maxCooldown = 2;
    targetAlly(true);
    targetEnemy(true);
  }

  public override void additionalEffects (BattleCharacter target) {
    Func<WardEffect> f = () => {
      WardEffect eff = new WardEffect();
      eff.duration = -1;
      return eff;
    };

    Aura<WardEffect> e = new Aura<WardEffect>(aoe, f);
    e.effectValue = level;
    e.duration = 2;
    e.applyToSelf = true;

    e.caster = self;
    target.applyEffect(e);
  }
}

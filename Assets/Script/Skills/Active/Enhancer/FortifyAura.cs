using UnityEngine;
using System;
using System.Collections.Generic;

public class FortifyAura: SelfTarget {
  public int aoe {get; set;}

  public override string animation { get { return "ClericCast"; }}
  public override Color castColor { get { return castColorEnhancer; }}

  public FortifyAura() {
    range = 0;
    useWepRange = false;
    aoe = 2;
    useLos = false;
    name = "FortifyAura";
    maxCooldown = 2;
    targetAlly(true);
    targetEnemy(false);
  }

  public override void additionalEffects (BattleCharacter target) {
    Func<FortifyEffect> f = () => {
      FortifyEffect eff = new FortifyEffect();
      eff.duration = -1;
      return eff;
    };

    Aura<FortifyEffect> e = new Aura<FortifyEffect>(aoe, f);
    e.level = level;
    e.duration = 2;
    e.applyToSelf = true;

    e.caster = self;
    target.applyEffect(e);
  }

}

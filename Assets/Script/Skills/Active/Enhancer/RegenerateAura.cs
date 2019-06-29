using UnityEngine;
using System;
using System.Collections.Generic;

public class RegenerateAura: SelfTarget {
  public int aoe {get; set;}

  public override string animation { get { return "ClericCast"; }}
  public override Color castColor { get { return castColorEnhancer; }}

  public RegenerateAura() {
    range = 0;
    useWepRange = false;
    aoe = 2;
    useLos = false;
    name = "RegenerateAura";
    maxCooldown = 2;
    targetAlly(true);
    targetEnemy(false);
  }

  public override void additionalEffects (BattleCharacter target) {
    Func<RegenerationEffect> f = () => {
      RegenerationEffect buff = new RegenerationEffect();
      buff.level = level;
      buff.healing = 2;
      buff.duration = -1;
      return buff;
    };

    Aura<RegenerationEffect> e = new Aura<RegenerationEffect>(aoe, f);
    e.level = level;
    e.duration = 2;
    e.applyToSelf = true;

    e.caster = self;
    target.applyEffect(e);
  }
}

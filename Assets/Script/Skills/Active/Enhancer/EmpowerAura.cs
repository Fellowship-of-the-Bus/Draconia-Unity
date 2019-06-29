using UnityEngine;
using System;
using System.Collections.Generic;

public class EmpowerAura: SelfTarget {
  public int aoe {get; set;}

  public override string animation { get { return "ClericCast"; }}
  public override Color castColor { get { return castColorEnhancer; }}

  public EmpowerAura() {
    range = 0;
    useWepRange = false;
    aoe = 2;
    useLos = false;
    name = "EmpowerAura";
    maxCooldown = 2;
    targetAlly(true);
    targetEnemy(false);
  }

  public override void additionalEffects (BattleCharacter target) {
    Func<EmpowerEffect> f = () => {
      EmpowerEffect eff = new EmpowerEffect();
      eff.duration = -1;
      return eff;
    };

    Aura<EmpowerEffect> e = new Aura<EmpowerEffect>(aoe, f);
    e.level = level;
    e.duration = 2;
    e.applyToSelf = true;

    e.caster = self;
    target.applyEffect(e);
  }

}

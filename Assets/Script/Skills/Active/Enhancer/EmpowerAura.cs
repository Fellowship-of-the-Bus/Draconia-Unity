using UnityEngine;
using System;
using System.Collections.Generic;

public class EmpowerAura: SelfTarget {
  public int aoe {get; set;}

  public EmpowerAura() {
    range = 0;
    useWepRange = false;
    aoe = 2;
    useLos = false;
    name = "EmpowerAura";
    maxCooldown = 2;
  }

  public override void additionalEffects (Character target) {
    Func<EmpowerEffect> f = () => {
      EmpowerEffect eff = new EmpowerEffect();
      eff.duration = -1;
      return eff;
    };

    Aura<EmpowerEffect> e = new Aura<EmpowerEffect>(aoe, f);
    e.level = level;
    e.duration = 2;
    e.applyToSelf = true;

    target.applyEffect(e);
  }

  public override int calculateDamage(Character source, Character target) {
    return 0;
  }
}
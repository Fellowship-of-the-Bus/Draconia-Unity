using UnityEngine;
using System;
using System.Collections.Generic;

public class Intercept: ActiveSkill {
  public int aoe {get; set;}

  public Intercept() {
    range = 0;
    useWepRange = false;
    aoe = 1;
    useLos = false;
    name = "Intercept";
    cooldown = 2;
  }

  public override void additionalEffects (Character target) {
    Func<InterceptEffect> f = () => {
      InterceptEffect eff = new InterceptEffect();
      eff.origin = self;
      eff.duration = -1;
      return eff;
    };

    Aura<InterceptEffect> e = new Aura<InterceptEffect>(aoe, f);
    e.level = level;
    e.duration = 2;
    e.applyToSelf = false;

    target.applyEffect(e);
  }

  public override List<GameObject> getTargets() {
    List<GameObject> targets = new List<GameObject>();
    targets.Add(self.gameObject);
    return targets;
  }

  public override int calculateDamage(Character source, Character target) {
    return 0;
  }
}

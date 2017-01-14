using UnityEngine;
using System;
using System.Collections.Generic;

public class HasteAura: PassiveSkill {
  public int aoe {get; set;}


  public override List<GameObject> getTargets() {
    List<GameObject> targets = new List<GameObject>();
    targets.Add(self.gameObject);
    return targets;
  }

  public override void whenApplied(Character c) {
    range = 0;
    aoe = 2;
    useLos = false;
    name = "HasteAura";

    Func<HasteEffect> f = () => {
      HasteEffect eff = new HasteEffect();
      eff.duration = -1;
      return eff;
    };

    Aura<HasteEffect> e = new Aura<HasteEffect>(aoe, f);
    e.level = level;
    e.duration = -1;
    e.applyToSelf = true;

    owner.applyEffect(e);

  }

  public override void onActivate() {
  }

  public override void onDeactivate() {
  }
}

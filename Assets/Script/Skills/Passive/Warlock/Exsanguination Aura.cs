using UnityEngine;
using System;
using System.Collections.Generic;

public class ExsanguinationAura: PassiveSkill {
  public int aoe {get; set;}

  public override List<GameObject> getTargets() {
    List<GameObject> targets = new List<GameObject>();
    targets.Add(self.gameObject);
    return targets;
  }

  protected override void onApply(Character c) {
    range = 0;
    aoe = 3;
    useLos = false;
    name = "ExsanguinationAura";

    Func<BleedEffect> f = () => {
      BleedEffect eff = new BleedEffect();
      eff.level = level;
      eff.duration = -1;
      eff.damage = owner.attr.intelligence;
      return eff;
    };

    Aura<BleedEffect> e = new Aura<BleedEffect>(aoe, f, true);
    e.level = level;
    e.duration = -1;
    e.applyToSelf = true;

    owner.applyEffect(e);
  }

  protected override void onActivate() {
  }

  protected override void onDeactivate() {
  }
}

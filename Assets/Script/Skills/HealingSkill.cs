using UnityEngine;
using System.Collections.Generic;

public abstract class HealingSkill : ActiveSkill {

  public override void activate(Character target) {
    target.takeHealing(calculateHealing(self,target));
    additionalEffects(target);
  }

  public override sealed int calculateDamage(Character source, Character target) {
    Debug.Assert(false);
    return 0;
  }

  public abstract int calculateHealing(Character source, Character target);
  public override void additionalEffects(Character target) {

  }
}

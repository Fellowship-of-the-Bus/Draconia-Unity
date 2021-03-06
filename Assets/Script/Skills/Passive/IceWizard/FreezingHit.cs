using UnityEngine;
using System.Collections.Generic;

public class FreezingHit : PassiveSkill {
  protected override void onActivate() {
    attachListener(self,EventHook.postAttack);
  }

  protected override void additionalEffect(Draconia.Event e) {
    float chance = UnityEngine.Random.value;
    if (e.hook == EventHook.postAttack && e.damageTaken > 0 && chance < (0.1 * level)) {
      SlowEffect debuff = new SlowEffect();
      debuff.effectValue = level*2;
      debuff.duration = 2;
      debuff.caster = self;
      e.attackTarget.applyEffect(debuff);
    }
  }

  protected override void onDeactivate() {
    detachListener(self);
  }
}

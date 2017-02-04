using UnityEngine;
using System.Collections.Generic;

public class FreezingHit : PassiveSkill {
  protected override void onActivate() {
    attachListener(self,EventHook.postAttack);
  }

  protected override void additionalEffect(Event e) {
    float chance = UnityEngine.Random.value;
    if (e.hook == EventHook.postAttack && e.damageTaken > 0 && chance < (0.1 * level)) {
      Debug.Log("Slowed " + e.attackTarget);
      SlowEffect debuff = new SlowEffect();
      debuff.level = level*2;
      debuff.duration = 2;
      e.attackTarget.applyEffect(debuff);
    }
  }

  protected override void onDeactivate() {
    detachListener(self);
  }
}

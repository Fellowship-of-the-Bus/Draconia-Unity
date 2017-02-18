using System;
using UnityEngine;

public class HealWhenHitEffect : DurationEffect {
  protected override void onActivate() {
    attachListener(owner, EventHook.postDamage);
  }
  protected override void onDeactivate() {
    detachListener(owner);
  }

  protected override void additionalEffect(Event e) {
    if (owner.isAlive()) {
      owner.takeHealing((int)(level*owner.healingMultiplier));
    }
  }
}

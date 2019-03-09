using System;
using UnityEngine;

public class HealWhenHitEffect : DurationEffect {
  protected override void onActivate() {
    attachListener(owner, EventHook.postDamage);
  }
  protected override void onDeactivate() {
    detachListener(owner);
  }

  protected override void additionalEffect(Draconia.Event e) {
    if (owner.isAlive()) {
      owner.takeHealing(owner.calculateHealing(level));
    }
  }
}

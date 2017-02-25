using System;
using UnityEngine;
using System.Collections.Generic;

public class DodgeEffect : DurationEffect {
  protected override void onActivate() {
    attachListener(owner, EventHook.preDamage);
  }
  protected override void onDeactivate() {
    detachListener(owner);
  }

  protected override void additionalEffect(Event e) {
    e.finishAttack = false;
    owner.removeEffect(this);
  }
}

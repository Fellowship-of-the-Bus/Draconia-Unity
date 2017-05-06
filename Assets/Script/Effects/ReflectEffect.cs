using System;
using UnityEngine;
using System.Collections.Generic;

public class ReflectEffect : DurationEffect {
  protected override void onActivate() {
    attachListener(owner, EventHook.preDamage);
  }
  protected override void onDeactivate() {
    detachListener(owner);
  }

  protected override void additionalEffect(Event e) {
    if (e.sender is BattleCharacter) {
      e.newTarget = e.sender;
      owner.removeEffect(this);
    }
  }
}

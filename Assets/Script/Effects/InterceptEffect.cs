using UnityEngine;
using System.Collections.Generic;
using System;

public class InterceptEffect : DurationEffect {
  public Character origin;

  public override void onActivate() {
    attachListener(owner, EventHook.preDamage);
  }
  public override void onDeactivateListeners() {
    detachListener(owner);
  }
  public override void additionalEffect(Event e) {
    e.newTarget = origin;
  }
}

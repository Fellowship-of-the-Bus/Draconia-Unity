using UnityEngine;
using System.Collections.Generic;
using System;

public class InterceptEffect : DurationEffect {
  public Character origin;

  protected override void onActivate() {
    attachListener(owner, EventHook.preDamage);
  }
  protected override void onDeactivateListeners() {
    detachListener(owner);
  }
  protected override void additionalEffect(Event e) {
    e.newTarget = origin;
  }
}

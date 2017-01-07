using UnityEngine;
using System.Collections.Generic;
using System;

public class InterceptEffect : Effect {
  public Character origin;

  public override void onActivate() {
    attachListener(owner, EventHook.preDamage);
    Debug.Log("applied intercept");
  }
  public override void onDeactivate() {
    detachListener(owner);
    Debug.Log("removed intercept");
  }
  public override void additionalEffect(Event e) {
    e.newTarget = origin;
  }
}

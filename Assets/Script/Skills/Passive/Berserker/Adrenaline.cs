using UnityEngine;
using System.Collections.Generic;
using System;

public class Adrenaline : PassiveSkill {
  public int maxCooldown = 4;
  int timeToActivate = 0;

  protected override void onActivate() {
    attachListener(owner, EventHook.preDeath);
    attachListener(owner, EventHook.endTurn);
  }
  protected override void onDeactivate() {
    detachListener(owner);
  }
  protected override void additionalEffect(Event e) {
    if (e.hook == EventHook.preDeath && timeToActivate == 0) {
      e.preventDeath = true;
      timeToActivate = maxCooldown;
    } else if (e.hook == EventHook.endTurn) {
      timeToActivate = (int)Math.Max(timeToActivate - 1, 0);
    }
  }
}

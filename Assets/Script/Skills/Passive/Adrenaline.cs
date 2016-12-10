using UnityEngine;
using System.Collections.Generic;
using System;

public class Adrenaline : PassiveSkill {
  public int cooldown = 4;
  int timeToActivate = 0;

  public override List<GameObject> getTargets() {
    List<GameObject> targets = new List<GameObject>();
    targets.Add(self.gameObject);
    return targets;
  }

  public override void onActivate() {
    attachListener(owner, EventHook.preDeath);
    attachListener(owner, EventHook.endTurn);
  }
  public override void onDeactivate() {
    detachListener(owner);
  }
  public override void additionalEffect(Event e) {
    if (e.hook == EventHook.preDeath && timeToActivate == 0) {
      e.preventDeath = true;
      timeToActivate = cooldown;
    } else if (e.hook == EventHook.endTurn) {
      timeToActivate = (int)Math.Max(timeToActivate - 1, 0);
    }
  }
}

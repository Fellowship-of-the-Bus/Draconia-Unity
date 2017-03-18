using UnityEngine;
using System.Collections.Generic;

public class Empower: SingleTarget {
  public Empower() {
    range = 5;
    useLos = false;
    name = "Empower";
    maxCooldown = 2;
  }

  public override void additionalEffects (Character target) {
    EmpowerEffect e = new EmpowerEffect();
    e.level = level;
    e.duration = 2;
    target.applyEffect(e);

    Event buffEvent = new Event(self, EventHook.useBuffSkill);
    buffEvent.appliedBuff = e;
    self.onEvent(buffEvent);
  }
}

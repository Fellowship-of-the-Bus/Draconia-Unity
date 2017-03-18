using UnityEngine;
using System.Collections.Generic;

public class Fortify: SingleTarget {
  public Fortify() {
    range = 5;
    useLos = false;
    name = "Fortify";
    maxCooldown = 2;
  }

  public override void additionalEffects (Character target) {
    FortifyEffect e = new FortifyEffect();
    e.level = level;
    e.duration = 2;
    target.applyEffect(e);


    Event buffEvent = new Event(self, EventHook.useBuffSkill);
    buffEvent.appliedBuff = e;
    self.onEvent(buffEvent);
  }

}

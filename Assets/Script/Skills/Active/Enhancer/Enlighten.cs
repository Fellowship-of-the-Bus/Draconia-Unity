using UnityEngine;
using System.Collections.Generic;

public class Enlighten: SingleTarget {
  public Enlighten() {
    range = 5;
    useLos = false;
    name = "Enlighten";
    maxCooldown = 2;
  }

  public override void additionalEffects (Character target) {
    EnlightenEffect e = new EnlightenEffect();
    e.level = level;
    e.duration = 2;
    target.applyEffect(e);


    Event buffEvent = new Event(self, EventHook.useBuffSkill);
    buffEvent.appliedBuff = e;
    self.onEvent(buffEvent);
  }

  public override int calculateDamage(Character source, Character target) {
    return 0;
  }
}
using UnityEngine;
using System.Collections.Generic;

public class Ward: SingleTarget {
  public Ward() {
    range = 5;
    useLos = false;
    name = "Ward";
    maxCooldown = 2;
  }

  public override void additionalEffects (BattleCharacter target) {
    WardEffect e = new WardEffect();
    e.level = level;
    e.duration = 2;
    target.applyEffect(e);


    Event buffEvent = new Event(self, EventHook.useBuffSkill);
    buffEvent.appliedBuff = e;
    self.onEvent(buffEvent);
  }
}

using UnityEngine;
using System.Collections.Generic;

public class Fortify: SingleTarget {

  public override string animation { get { return "ClericCast"; }}

  public Fortify() {
    range = 5;
    useLos = false;
    name = "Fortify";
    maxCooldown = 2;
    canTargetSelf = true;
    targetAlly(true);
    targetEnemy(false);
  }

  public override void additionalEffects (BattleCharacter target) {
    FortifyEffect e = new FortifyEffect();
    e.level = level;
    e.duration = 2;
    target.applyEffect(e);


    Event buffEvent = new Event(self, EventHook.useBuffSkill);
    buffEvent.appliedBuff = e;
    self.onEvent(buffEvent);
  }

}

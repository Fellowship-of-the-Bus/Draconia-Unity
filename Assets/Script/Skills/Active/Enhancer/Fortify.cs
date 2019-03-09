using UnityEngine;
using System.Collections.Generic;

public class Fortify: SingleTarget {

  public override string animation { get { return "ClericCast"; }}
  public override Color castColor { get { return castColorEnhancer; }}

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


    Draconia.Event buffEvent = new Draconia.Event(self, EventHook.useBuffSkill);
    buffEvent.appliedBuff = e;
    self.onEvent(buffEvent);
  }

}

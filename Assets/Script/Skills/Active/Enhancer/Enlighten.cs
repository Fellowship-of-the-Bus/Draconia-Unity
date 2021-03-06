using UnityEngine;
using System.Collections.Generic;

public class Enlighten: SingleTarget {

  public override string animation { get { return "ClericCast"; }}
  public override Color castColor { get { return castColorEnhancer; }}

  public Enlighten() {
    range = 5;
    useLos = false;
    name = "Enlighten";
    maxCooldown = 2;
    canTargetSelf = true;
    targetAlly(true);
    targetEnemy(false);
  }

  public override void additionalEffects (BattleCharacter target) {
    EnlightenEffect e = new EnlightenEffect();
    e.effectValue = level;
    e.duration = 2;
    e.caster = self;
    target.applyEffect(e);


    Draconia.Event buffEvent = new Draconia.Event(self, EventHook.useBuffSkill);
    buffEvent.appliedBuff = e;
    self.onEvent(buffEvent);
  }

}

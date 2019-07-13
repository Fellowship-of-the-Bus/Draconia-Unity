using UnityEngine;
using System.Collections.Generic;

public class Empower: SingleTarget {

  public override string animation { get { return "ClericCast"; }}
  public override Color castColor { get { return castColorEnhancer; }}

  public Empower() {
    range = 5;
    useLos = false;
    name = "Empower";
    maxCooldown = 2;
    canTargetSelf = true;
    targetAlly(true);
    targetEnemy(false);
  }

  public override void additionalEffects (BattleCharacter target) {
    EmpowerEffect e = new EmpowerEffect();
    e.effectValue = level;
    e.duration = 2;
    e.caster = self;
    target.applyEffect(e);

    Draconia.Event buffEvent = new Draconia.Event(self, EventHook.useBuffSkill);
    buffEvent.appliedBuff = e;
    self.onEvent(buffEvent);
  }
}

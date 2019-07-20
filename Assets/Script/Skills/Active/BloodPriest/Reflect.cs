using UnityEngine;
using System.Collections.Generic;

public class Reflect: SingleTarget {

  public override string animation { get { return "Cast"; }}
  public override Color castColor { get { return castColorBloodPriest; }}

  public Reflect() {
    range = 5;
    useLos = false;
    name = "Reflect";
    maxCooldown = 2;
    canTargetSelf = true;
    targetAlly(true);
    targetEnemy(false);
  }

  public override void additionalEffects (BattleCharacter target) {
    int cost = self.maxHealth / 10;
    if (self.curHealth > cost) {
      self.takeDamage(cost,self);
      ReflectEffect e = new ReflectEffect();
      e.duration = 2;
      e.caster = self;
      target.applyEffect(e);


      Draconia.Event buffEvent = new Draconia.Event(self, EventHook.useBuffSkill);
      buffEvent.appliedBuff = e;
      self.onEvent(buffEvent);
    }
  }

}

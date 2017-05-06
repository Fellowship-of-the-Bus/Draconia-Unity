using UnityEngine;
using System.Collections.Generic;

public class Reflect: SingleTarget {
  public Reflect() {
    range = 5;
    useLos = false;
    name = "Reflect";
    maxCooldown = 2;
  }

  public override void additionalEffects (BattleCharacter target) {
    int cost = self.maxHealth / 10;
    if (self.curHealth > cost) {
      self.takeDamage(cost);
      ReflectEffect e = new ReflectEffect();
      e.level = level;
      e.duration = 2;
      target.applyEffect(e);


      Event buffEvent = new Event(self, EventHook.useBuffSkill);
      buffEvent.appliedBuff = e;
      self.onEvent(buffEvent);
    }
  }

}

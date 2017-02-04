using System;
using System.Collections.Generic;
using UnityEngine;

public class CounterSkill : PassiveSkill {
  protected override void onActivate() {
    attachListener(owner, EventHook.postDamage);
  }
  protected override void onDeactivate() {
    detachListener(owner);
  }

  protected override void additionalEffect(Event e) {
    // don't counter yourself or your teammates.
    if (owner == e.sender || owner.team == e.sender.team) return;
    float chance = UnityEngine.Random.value;
    if (chance < 0.1*level && owner.inRange(e.sender, 1)) {
      Punch atk = new Punch();
      atk.level = level;
      atk.self = owner;

      List<Effected> target = new List<Effected>();
      target.Add(e.sender);
      owner.attackWithSkill(atk, target);
    }
  }
}

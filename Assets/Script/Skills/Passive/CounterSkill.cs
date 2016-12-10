using System;
using System.Collections.Generic;
using UnityEngine;

public class CounterSkill : PassiveSkill {
  public override List<GameObject> getTargets() {
    List<GameObject> targets = new List<GameObject>();
    targets.Add(self.gameObject);
    return targets;
  }

  public override void onActivate() {
    attachListener(owner, EventHook.postDamage);
  }
  public override void onDeactivate() {
    detachListener(owner);
  }

  public override void additionalEffect(Event e) {
    float chance = UnityEngine.Random.value;
    if (chance < 0.1*level && owner.inRange(e.sender, 1)) {
      Punch atk = new Punch();
      atk.level = level;
      atk.self = owner;

      List<Character> target = new List<Character>();
      target.Add(e.sender);
      owner.attackWithSkill(atk, target);
    }
  }
}

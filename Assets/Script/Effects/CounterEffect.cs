using UnityEngine;
using System.Collections.Generic;

public class CounterEffect : Effect {
  public override void onActivate() {
    attachListener(owner, EventHook.postDamage);
  }
  public override void onDeactivate() {
    detachListener(owner);
  }

  public override void additionalEffect(Event e) {
    float chance = Random.value;
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

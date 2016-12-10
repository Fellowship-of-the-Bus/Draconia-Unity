using System;
using UnityEngine;
using System.Collections.Generic;

public class Dodge : PassiveSkill {
  public override List<GameObject> getTargets() {
    List<GameObject> targets = new List<GameObject>();
    targets.Add(self.gameObject);
    return targets;
  }

  public override void onActivate() {
    attachListener(owner, EventHook.preDamage);
  }
  public override void onDeactivate() {
    detachListener(owner);
  }

  public override void additionalEffect(Event e) {
    float chance = UnityEngine.Random.value;
    if (chance < 0.1*level) {
      e.finishAttack = false;
//      Debug.Log("dodged: " + e.finishAttack);
    } else {
//      Debug.Log("did not dodge: " + e.finishAttack);
    }
  }
}

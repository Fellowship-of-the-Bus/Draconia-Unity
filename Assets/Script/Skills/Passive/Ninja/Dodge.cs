using System;
using UnityEngine;
using System.Collections.Generic;

public class Dodge : PassiveSkill {
  protected override void onActivate() {
    attachListener(owner, EventHook.preDamage);
  }
  protected override void onDeactivate() {
    detachListener(owner);
  }

  protected override void additionalEffect(Draconia.Event e) {
    float chance = UnityEngine.Random.value;
    if (chance < 0.1*level) {
      e.finishAttack = false;
    }
  }
}

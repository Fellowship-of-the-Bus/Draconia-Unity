using UnityEngine;
using System.Collections.Generic;
using System;

public class Inspiration : PassiveSkill {

  protected override void onActivate() {
    attachListener(owner, EventHook.postAttack);
  }
  protected override void onDeactivate() {
    detachListener(owner);
  }
  protected override void additionalEffect(Draconia.Event e) {
    float chance = UnityEngine.Random.value;
    if (chance < 0.5f*level) {
      e.skillUsed.curCooldown = 0;
    }
  }
}

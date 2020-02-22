using System;
using UnityEngine;
using System.Collections.Generic;

public class Dodge : PassiveSkill {
  public Dodge() {
    name = "Dodge";
  }

  public override string tooltipDescription { get {
    return (100 * triggerChance()).ToString() + "% chance to avoid damage." ;
  }}

  protected override void onActivate() {
    attachListener(owner, EventHook.preDamage);
  }
  protected override void onDeactivate() {
    detachListener(owner);
  }

  protected override void additionalEffect(Draconia.Event e) {
    float chance = UnityEngine.Random.value;
    if (chance < triggerChance()) {
      e.finishAttack = false;
    }
  }

  float triggerChance() {
    return 0.1f*level;
  }
}

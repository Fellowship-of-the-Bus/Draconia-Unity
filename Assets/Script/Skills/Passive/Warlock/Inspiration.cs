using UnityEngine;
using System.Collections.Generic;
using System;

public class Inspiration : PassiveSkill {
  public Inspiration() {
    name = "Inspiration";
  }

  public override string tooltipDescription { get {
    return "Grants a " + (100 * triggerChance()).ToString() + "% chance not to trigger the cooldown of a skill that was used.";
  }}

  protected override void onActivate() {
    attachListener(owner, EventHook.postAttack);
  }
  protected override void onDeactivate() {
    detachListener(owner);
  }
  protected override void additionalEffect(Draconia.Event e) {
    float chance = UnityEngine.Random.value;
    if (chance < triggerChance()) {
      e.skillUsed.curCooldown = 0;
    }
  }

  float triggerChance() {
    return 0.5f*level;
  }
}

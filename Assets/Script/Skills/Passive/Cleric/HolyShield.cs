using UnityEngine;
using System.Collections.Generic;
using System;

public class HolyShield : PassiveSkill {
  protected override void onActivate() {
    attachListener(owner, EventHook.postHealing);
  }
  protected override void onDeactivate() {
    detachListener(owner);
  }
  protected override void additionalEffect(Event e) {
    if (e.healingDone > 0) {
      BattleCharacter target = e.healTarget;
      FortifyEffect effect = new FortifyEffect();
      effect.level = level;
      effect.duration = 2;
      target.applyEffect(effect);
    }
  }
}

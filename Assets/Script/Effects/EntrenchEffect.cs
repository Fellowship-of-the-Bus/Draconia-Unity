﻿using UnityEngine;
using System;

public class EntrenchEffect : DurationEffect {
  protected override void onActivate() {
    attachListener(GameManager.get.eventManager, EventHook.preMove);
    attachListener(GameManager.get.eventManager, EventHook.cancel);
    owner.weapon.range += this.level;
  }
  protected override void onDeactivateEffects() {
    detachListener(GameManager.get.eventManager);
  }

  protected override void additionalEffect(Event e) {
    if (e.hook == EventHook.preMove && e.sender == owner) {
      if (duration == -1) {
        duration = 1;
        owner.weapon.range -= this.level; 
      }
    } else if (e.hook == EventHook.cancel && e.sender == owner) {
      if (duration == 1) {
        duration = -1;
        owner.weapon.range += this.level; 
      }
    }
  }
}

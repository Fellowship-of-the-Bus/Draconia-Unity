﻿using UnityEngine;

public class DodgeEffect : Effect {
  public override void onRemove() {

  }
  public override void onActivate() {
    attachListener(owner, EventHook.preAttack);
  }
  public override void onDeactivate() {
    detachListener(owner);
  }

  public override void onEvent(Event e) {
    float chance = Random.value;
    if (chance < .3) {
      GameManager.get.eventManager.onEvent(new Event(owner, EventHook.dodge));
    }
  }
}

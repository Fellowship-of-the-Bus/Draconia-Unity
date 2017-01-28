using System;
using UnityEngine;

public class DisableEffect : Effect {
  public ActiveSkill origin;
  public ActiveSkill target;
  protected override void onActivate() {
    target.setCooldown();
    origin.cooldown = ActiveSkill.InfiniteCooldown;
    attachListener(GameManager.get.eventManager, EventHook.startTurn);
  }
  protected override void onDeactivate() {
    detachListener(GameManager.get.eventManager);
    origin.cooldown = 0;
    target.cooldown = 0;
  }

  protected override void additionalEffect(Event e) {
    if (target.cooldown == 0) {
      // end cooldown of skill that applied this effect
      origin.cooldown = 0;
      owner.removeEffect(this);
    }
  }
}

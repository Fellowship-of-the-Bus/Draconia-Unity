using System;
using UnityEngine;

public class DisableEffect : Effect {
  public ActiveSkill origin;
  public ActiveSkill target;
  protected override void onActivate() {
    target.setCooldown();
    origin.maxCooldown = ActiveSkill.InfiniteCooldown;
    attachListener(GameManager.get.eventManager, EventHook.startTurn);
  }
  protected override void onDeactivate() {
    detachListener(GameManager.get.eventManager);
    origin.maxCooldown = 0;
    target.maxCooldown = 0;
  }

  protected override void additionalEffect(Draconia.Event e) {
    if (target.maxCooldown == 0) {
      // end maxCooldown of skill that applied this effect
      origin.maxCooldown = 0;
      owner.removeEffect(this);
    }
  }
}

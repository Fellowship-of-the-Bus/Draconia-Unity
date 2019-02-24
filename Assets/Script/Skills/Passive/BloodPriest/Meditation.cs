using System;

public class Meditation : PassiveSkill {
  protected override void onActivate() {
    attachListener(owner, EventHook.endTurn);
  }
  protected override void onDeactivate() {
    detachListener(owner);
  }

  protected override void additionalEffect(Draconia.Event e) {
    owner.takeHealing((int)(1*owner.healingMultiplier));
  }
}

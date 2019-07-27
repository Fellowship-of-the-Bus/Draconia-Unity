using UnityEngine;

public class RegenerationEffect : DurationEffect, HealthChangingEffect {

  protected override void onActivate() {
    attachListener(owner, EventHook.endTurn);
  }
  protected override void onDeactivateListeners() {
    detachListener(owner);
  }
  protected override void additionalEffect(Draconia.Event e) {
    owner.takeHealing(owner.calculateHealing(effectValue));
  }

  public int healthChange() {
    return effectValue;
  }
}

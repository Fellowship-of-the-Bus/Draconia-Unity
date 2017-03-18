using UnityEngine;

public class RegenerationEffect : DurationEffect {
  public int healing;

  protected override void onActivate() {
    attachListener(owner, EventHook.endTurn);
  }
  protected override void onDeactivateListeners() {
    detachListener(owner);
  }
  protected override void additionalEffect(Event e) {
    owner.takeHealing(owner.calculateHealing(healing));
  }
  protected override bool isGreaterThan(Effect e) {
    Debug.Assert(e is RegenerationEffect);
    if (e is RegenerationEffect) {
      return this.healing > (e as RegenerationEffect).healing;
    } else {
      //should only be comparing regeneration effects, shouldn't ever get here.
      return true;
    }
  }
}

using UnityEngine;

public class RegenerationEffect : DurationEffect, HealthChangingEffect {
  public int healing;

  protected override void onActivate() {
    attachListener(owner, EventHook.endTurn);
  }
  protected override void onDeactivateListeners() {
    detachListener(owner);
  }
  protected override void additionalEffect(Draconia.Event e) {
    owner.takeHealing(owner.calculateHealing(healing));
  }
  public override int CompareTo(Effect e) {
    Debug.Assert(e is RegenerationEffect);
    if (e is RegenerationEffect) {
      return this.healing.CompareTo((e as RegenerationEffect).healing);
    } else {
      //should only be comparing regeneration effects, shouldn't ever get here.
      return base.CompareTo(e);
    }
  }

  public int healthChange() {
    return healing;
  }
}

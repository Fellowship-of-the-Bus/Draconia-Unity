using UnityEngine;

public class BleedEffect : DurationEffect {
  public int damage;

  protected override void onActivate() {
    attachListener(owner, EventHook.endTurn);
  }
  protected override void onDeactivateListeners() {
    detachListener(owner);
  }
  protected override void additionalEffect(Event e) {
    owner.takeDamage(damage);
  }
  protected override bool isGreaterThan(Effect e) {
    Debug.Assert(e is BleedEffect);
    if (e is BleedEffect) {
      return this.damage > (e as BleedEffect).damage;
    } else {
      //should only be comparing bleed effects, shouldn't ever get here.
      return true;
    }
  }
}

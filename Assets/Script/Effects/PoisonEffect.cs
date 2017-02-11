using UnityEngine;

public class PoisonEffect : DurationEffect {
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
    Debug.Assert(e is PoisonEffect);
    if (e is PoisonEffect) {
      return this.damage > (e as PoisonEffect).damage;
    } else {
      //should only be comparing Poison effects, shouldn't ever get here.
      return true;
    }
  }
}

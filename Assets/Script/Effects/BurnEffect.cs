using UnityEngine;

public class BurnEffect : DurationEffect {
  public int damage;
  public float multiplier = 1.0f;

  protected override void onActivate() {
    attachListener(owner, EventHook.endTurn);
  }
  protected override void onDeactivateListeners() {
    detachListener(owner);
  }
  protected override void additionalEffect(Event e) {
    owner.takeDamage((int)(damage * multiplier));
    multiplier -= 0.15f;
  }
  protected override bool isGreaterThan(Effect e) {
    Debug.Assert(e is BurnEffect);
    if (e is BurnEffect) {
      return this.damage > (e as BurnEffect).damage;
    } else {
      //should only be comparing Burn effects, shouldn't ever get here.
      return true;
    }
  }
}

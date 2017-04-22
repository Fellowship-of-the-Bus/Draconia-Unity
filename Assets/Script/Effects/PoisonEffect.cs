using UnityEngine;

public class PoisonEffect : DurationEffect {
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
    multiplier += 0.15f;
  }
  public override int CompareTo(Effect e) {
    Debug.Assert(e is PoisonEffect);
    if (e is PoisonEffect) {
      return this.damage.CompareTo((e as PoisonEffect).damage);
    } else {
      //should only be comparing Poison effects, shouldn't ever get here.
      return base.CompareTo(e);
    }
  }
}

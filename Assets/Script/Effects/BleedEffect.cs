using UnityEngine;

public class BleedEffect : DurationEffect, HealthChangingEffect {
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
  public override int CompareTo(Effect e) {
    Debug.Assert(e is BleedEffect);
    if (e is BleedEffect) {
      return this.damage.CompareTo((e as BleedEffect).damage);
    } else {
      //should only be comparing bleed effects, shouldn't ever get here.
      return base.CompareTo(e);
    }
  }

  public int healthChange() {
    return -damage;
  }
}

using UnityEngine;

public class BleedEffect : Effect {
  public int damage;
  public override void onRemove() {

  }
  public override void onActivate() {
    attachListener(owner, EventHook.endTurn);
  }
  public override void onDeactivate() {
    detachListener(owner);
  }
  public override void onEvent(Event e) {
    if (duration == 0) {
      //owner.removeEffect(this);
    } else {
      owner.takeDamage(damage);
      duration--;
    }
  }
  public override bool isGreaterThan(Effect e) {
    if (e is BleedEffect) {
      return this.damage > (e as BleedEffect).damage;
    } else {
      //should only be comparing bleed effects, shouldn't ever get here.
      return true;
    }
  }
}

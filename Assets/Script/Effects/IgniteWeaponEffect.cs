using UnityEngine;

public class IgniteWeaponEffect : DurationEffect {

  public override void onActivate() {
    attachListener(owner, EventHook.postAttack);
  }
  public override void onDeactivateListeners() {
    detachListener(owner);
  }
  public override void additionalEffect(Event e) {
    if (e.attackTarget.team != owner.team && e.damageTaken > 0) {
      e.attackTarget.takeDamage((int)(2*e.attackTarget.fireResMultiplier));
    }
  }
}

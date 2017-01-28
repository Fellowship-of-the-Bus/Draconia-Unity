using UnityEngine;

public class IgniteWeaponEffect : DurationEffect {

  protected override void onActivate() {
    attachListener(owner, EventHook.postAttack);
  }
  protected override void onDeactivateListeners() {
    detachListener(owner);
  }
  protected override void additionalEffect(Event e) {
    if (e.attackTarget.team != owner.team && e.damageTaken > 0) {
      e.attackTarget.takeDamage((int)(2*e.attackTarget.fireResMultiplier));
    }
  }
}

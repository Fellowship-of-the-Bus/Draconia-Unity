using UnityEngine;

public class IgniteWeaponEffect : DurationEffect {
  public IgniteWeaponEffect() {
    name = "Flaming Weapon";
  }

  DamageElement originalEle;
  protected override void onActivate() {
    attachListener(owner, EventHook.preSkill);
    attachListener(owner, EventHook.postSkill);
  }
  protected override void onDeactivateListeners() {
    detachListener(owner);
  }
  protected override void additionalEffect(Draconia.Event e) {
    if(e.hook == EventHook.preSkill) {
      ActiveSkill skill = e.skillUsed;
      if (skill.dType == DamageType.physical) {
        originalEle = skill.dEle;
        skill.dEle = DamageElement.fire;
        owner.attrChange.strength += 10;
      }
    } else if (e.hook == EventHook.postSkill) {
      ActiveSkill skill = e.skillUsed;
      if (skill.dType == DamageType.physical) {
        skill.dEle = originalEle;
        owner.attrChange.strength -= 10;
      }
    }
  }
}

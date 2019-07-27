using UnityEngine;
using System.Collections.Generic;

public class BrainFreeze: SingleTarget {
  public override string animation { get { return "Cast"; }}
  public override Color castColor { get { return castColorIceWizard; }}

  protected override string tooltipDescription { get {
    return "Rapily chill the mind of the target, dealing " + tooltipDamage + " damage and delaying their cooldowns\n"
      + "Ignores line of sight";
  }}

  public BrainFreeze() {
    useWepRange = false;
    useLos = false;
    name = "Brain Freeze";
    maxCooldown = 2;
    range = 3;

    dType = DamageType.magical;
    dEle = DamageElement.ice;

    targetAlly(false);
    targetEnemy(true);
  }

  public override void additionalEffects (BattleCharacter target) {
    foreach (ActiveSkill a in target.equippedSkills) {
      if (a.curCooldown > 0) a.curCooldown += level;
    }
  }
}

using UnityEngine;
using System.Collections.Generic;
using System;

public class FrostArmor: SingleTarget {

  public override string animation { get { return "Cast"; }}
  public override Color castColor { get { return castColorIceWizard; }}

  public FrostArmor() {
    range = 3;
    useWepRange = false;
    useLos = false;
    name = "Frost Armor";
    maxCooldown = 2;
    canTargetSelf = true;
    targetAlly(true);
    targetEnemy(false);
  }

  protected override string tooltipDescription { get {
    return "Shield the target with armor of magical ice";
  }}

  public override void additionalEffects (BattleCharacter target) {
    FrostArmorEffect debuff = new FrostArmorEffect();
    debuff.level = level;
    debuff.duration = 3;
    debuff.caster = self;
    target.applyEffect(debuff);
  }

}

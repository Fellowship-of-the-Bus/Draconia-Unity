using System;
using UnityEngine;
using System.Collections.Generic;

public class Disable : SingleTarget {

  public override string animation { get { return "Cast"; }}
  public override Color castColor { get { return castColorWarlock; }}

  public Disable() {
    name = "Disable";
    range = 3;
    useLos = false;
    maxCooldown = 0;
    targetAlly(false);
    targetEnemy(true);
  }

  public override string tooltipDescription { get {
    return "Disable a skill on the target";
  }}

  // TODO: UI to select skill
  public override void additionalEffects (BattleCharacter target) {
    DisableEffect debuff = new DisableEffect();
    debuff.origin = this;
    debuff.target = target.equippedSkills[0]; // TODO: fix this to use the skill that the player selected
    debuff.caster = self;
    target.applyEffect(debuff);
  }
}


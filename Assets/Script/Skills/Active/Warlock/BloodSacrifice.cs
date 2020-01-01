using UnityEngine;
using System.Collections.Generic;
using System;

public class BloodSacrifice : SelfTarget {

  public override string animation { get { return "Cast"; }}
  public override Color castColor { get { return castColorWarlock; }}

  public BloodSacrifice() {
    range = 0;
    useWepRange = false;
    useLos = false;
    name = "Blood Sacrifice";
    maxCooldown = 2;
    targetAlly(true);
    targetEnemy(false);
  }

  protected override string tooltipDescription { get {
    return "Sacrifice 2 health to gain bonus stats for 2 turns";
  }}

  public override void additionalEffects(BattleCharacter target) {
    BloodSacrificeEffect e = new BloodSacrificeEffect();
    e.setValue(level);
    e.duration = 2;
    e.caster = self;
    target.applyEffect(e);
    target.takeDamage(2,self);
  }
}

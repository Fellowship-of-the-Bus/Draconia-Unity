using UnityEngine;
using System.Collections.Generic;
using System;

public class BloodSacrifice : SelfTarget {

  public override string animation { get { return "Cast"; }}

  public BloodSacrifice() {
    range = 0;
    useWepRange = false;
    useLos = false;
    name = "Blood Sacrifice";
    maxCooldown = 2;
    targetAlly(true);
    targetEnemy(false);
  }

  public override void additionalEffects(BattleCharacter target) {
    BloodSacrificeEffect e = new BloodSacrificeEffect();
    e.setLevel(level);
    e.duration = 2;
    target.applyEffect(e);
    target.takeDamage(2);
  }
}

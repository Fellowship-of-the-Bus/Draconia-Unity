using UnityEngine;
using System.Collections.Generic;

public class Agility: SingleTarget {

  public override string animation { get { return "ClericCast"; }}
  public override Color castColor { get { return castColorEnhancer; }}

  public Agility() {
    range = 5;
    useLos = false;
    name = "Agility";
    maxCooldown = 2;
    canTargetSelf = true;
    targetAlly(true);
    targetEnemy(false);
  }

  public override void additionalEffects (BattleCharacter target) {
    ClimbEffect e = new ClimbEffect();
    e.duration = 1;
    e.caster = self;
    target.applyEffect(e);
  }
}

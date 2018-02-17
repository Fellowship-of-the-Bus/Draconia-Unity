using UnityEngine;
using System.Collections.Generic;

public class Agility: SingleTarget {

  public override string animation { get { return "ClericCast"; }}

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
    e.level = level;
    e.duration = 1;
    target.applyEffect(e);
  }
}

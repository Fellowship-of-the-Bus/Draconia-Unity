using UnityEngine;
using System.Collections.Generic;

public class Agility: SingleTarget {
  public Agility() {
    range = 5;
    useLos = false;
    name = "Agility";
    maxCooldown = 2;
  }

  public override void additionalEffects (Character target) {
    ClimbEffect e = new ClimbEffect();
    e.level = level;
    e.duration = 1;
    target.applyEffect(e);
  }

  public override int calculateDamage(Character source, Character target) {
    return 0;
  }
}

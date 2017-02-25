using UnityEngine;
using System.Collections.Generic;

public class Empower: SingleTarget {
  public Empower() {
    range = 5;
    useLos = false;
    name = "Empower";
    maxCooldown = 2;
  }

  public override void additionalEffects (Character target) {
    EmpowerEffect e = new EmpowerEffect();
    e.level = level;
    e.duration = 2;
    target.applyEffect(e);
  }

  public override int calculateDamage(Character source, Character target) {
    return 0;
  }
}

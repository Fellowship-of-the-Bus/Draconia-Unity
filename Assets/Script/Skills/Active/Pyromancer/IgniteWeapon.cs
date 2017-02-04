using UnityEngine;
using System.Collections.Generic;

public class IgniteWeapon: SingleTarget {
  public IgniteWeapon() {
    range = 1;
    useLos = false;
    name = "IgniteWeapon";
    maxCooldown = 2;
  }

  public override int calculateDamage(Character source, Character target) {
    return 0;
  }

  public override void additionalEffects (Character target) {
    IgniteWeaponEffect buff = new IgniteWeaponEffect();
    buff.level = level;
    buff.duration = 2;
    target.applyEffect(buff);
  }

}

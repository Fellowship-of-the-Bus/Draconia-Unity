using UnityEngine;
using System.Collections.Generic;

public class IgniteWeapon: SingleTarget {
  public IgniteWeapon() {
    range = 1;
    useLos = false;
    name = "IgniteWeapon";
    maxCooldown = 2;
    canTargetSelf = true;
  }

  public override void additionalEffects (BattleCharacter target) {
    IgniteWeaponEffect buff = new IgniteWeaponEffect();
    buff.level = level;
    buff.duration = 2;
    target.applyEffect(buff);
  }

}

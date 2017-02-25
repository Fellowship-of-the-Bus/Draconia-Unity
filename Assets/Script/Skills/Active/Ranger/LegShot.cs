﻿using UnityEngine;
using System.Collections.Generic;

public class LegShot: SingleTarget {
  public LegShot() {
    requireWeapon(Weapon.kinds.Ranged);
    useWepRange = true;
    useLos = true;
    name = "Leg Shot";
  }

  public override int calculateDamage(Character source, Character target) {
    return (int)(source.strength*(1+level*0.1) - target.physicalDefense);
  }

  public override void additionalEffects (Character target) {
    CrippleEffect debuff = new CrippleEffect();
    debuff.level = level;
    debuff.duration = 2;
    target.applyEffect(debuff);
  }
}
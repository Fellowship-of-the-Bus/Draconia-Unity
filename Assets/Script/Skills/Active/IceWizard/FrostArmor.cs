using UnityEngine;
using System.Collections.Generic;
using System;

public class FrostArmor: SingleTarget {
  public FrostArmor() {
    range = 3;
    useWepRange = false;
    useLos = false;
    name = "Frost Armor";
    cooldown = 2;
  }

  public override int calculateDamage(Character source, Character target) {
    return 0;
  }

  public override void additionalEffects (Character target) {
    FrostArmorEffect debuff = new FrostArmorEffect();
    debuff.level = level;
    debuff.duration = 3;
    target.applyEffect(debuff);
  }

}

using UnityEngine;
using System.Collections.Generic;

public class Cripple: SingleTarget {
  public Cripple() {
    requireMelee();
    useWepRange = true;
    useLos = false;
    name = "Cripple";
    cooldown = 2;
  }

  public override int calculateDamage(Character source, Character target) {
    return (int)(source.strength*(0.5+level*0.05) - target.physicalDefense);
  }

  public override void additionalEffects (Character target) {
    CrippleEffect debuff = new CrippleEffect();
    debuff.level = level;
    debuff.duration = 2;
    target.applyEffect(debuff);
  }
}

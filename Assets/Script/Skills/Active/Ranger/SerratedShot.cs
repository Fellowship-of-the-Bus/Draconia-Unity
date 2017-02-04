using UnityEngine;
using System.Collections.Generic;

public class SerratedShot: SingleTarget {
  public SerratedShot() {
    requireWeapon(Weapon.kinds.Ranged);
    useWepRange = true;
    useLos = true;
    name = "Serrated Shot";
    cooldown = 2;
  }

  public override int calculateDamage(Character source, Character target) {
    return (int)(source.strength*(0.5+level*0.05) - target.physicalDefense);
  }

  public override void additionalEffects (Character target) {
    BleedEffect debuff = new BleedEffect();
    debuff.level = level;
    debuff.duration = (level+5)/2;
    debuff.damage = (int)System.Math.Max((int)calculateDamage(self, target)*(0.2f + 0.1f*level), 1);
    target.applyEffect(debuff);
  }
}

using UnityEngine;
using System.Collections.Generic;

public class DarkPower: CircleAoE {
  public new bool targetsTiles = true;

  public DarkPower() {
    range = 3;
    useWepRange = false;
    aoe = 2;
    useLos = false;
    name = "Dark Power";
    effectsTiles = false;
    maxCooldown = 2;
  }

  public override void additionalEffects (Character target) {
    int cost = self.maxHealth / 10;
    if (self.curHealth > cost) {
      self.takeDamage(cost);

      if (target.team == self.team) {
        BloodSacrificeEffect e = new BloodSacrificeEffect();
        e.setLevel(level);
        e.duration = 2;
        target.applyEffect(e);
      } else {
        BleedEffect debuff = new BleedEffect();
        debuff.level = level;
        debuff.duration = (level+5)/2;
        //some damage or debuff etc..
        debuff.damage = (int)System.Math.Max((int)calculateDamage(target)*(0.2f + 0.1f*level), 1);
        target.applyEffect(debuff);
      }
    }
  }
}

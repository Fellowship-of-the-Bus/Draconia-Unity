using UnityEngine;
using System.Collections.Generic;

public class LifeDrain: SingleTarget {
  public LifeDrain() {
    range = 3;
    useWepRange = false;
    useLos = false;
    name = "Life Drain";
    cooldown = 2;
  }

  public override int calculateDamage(Character source, Character target) {
    return (int)(source.intelligence*(1+level*0.1) - target.magicDefense);
  }

  public override void additionalEffects(Character target) {
    self.takeHealing(calculateDamage(self,target));
  }



}

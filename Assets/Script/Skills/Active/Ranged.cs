using UnityEngine;
using System.Collections.Generic;

public class Ranged: RangedTargeting {
  public Ranged() {
    useLos = false;
    name = "Ranged";
  }

  public override int calculateDamage(Character source, Character target) {
    return (int)(source.attr.strength*(1+level*0.1) - target.attr.physicalDefense);
  }
}

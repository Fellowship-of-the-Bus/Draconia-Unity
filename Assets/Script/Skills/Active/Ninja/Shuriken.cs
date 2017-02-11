using UnityEngine;
using System.Collections.Generic;

public class Shuriken: SingleTarget {
  public Shuriken() {
    range = 3;
    useLos = true;
    name = "Shuriken";
    maxCooldown = 2;
  }

  public override int calculateDamage(Character source, Character target) {
    return (int)(source.strength*(1+level*0.1) - target.physicalDefense);
  }
}

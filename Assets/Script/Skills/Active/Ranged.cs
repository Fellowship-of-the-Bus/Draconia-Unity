using UnityEngine;
using System.Collections.Generic;

public class Ranged: SingleTarget {
  public Ranged() {
    useWepRange = true;
    useLos = true;
    name = "Ranged";
    cooldown = 2;
  }

  public override int calculateDamage(Character source, Character target) {
    return (int)(source.attr.strength*(1+level*0.1) - target.attr.physicalDefense);
  }
}

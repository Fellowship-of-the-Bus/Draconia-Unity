using UnityEngine;
using System.Collections.Generic;

public class Punch: SingleTarget {
  public Punch() {
    range = 1;
    useWepRange = false;
    useLos = false;
    name = "Punch";
    cooldown = 2;
  }

  public override int calculateDamage(Character source, Character target) {
    return (int)(source.attr.strength*(1+level*0.1) - target.attr.physicalDefense);
  }
}

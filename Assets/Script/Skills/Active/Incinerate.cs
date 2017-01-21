using UnityEngine;
using System.Collections.Generic;

public class Incinerate: SingleTarget {
  public Incinerate() {
    range = 3;
    useWepRange = false;
    useLos = false;
    name = "Incinerate";
    cooldown = 2;
  }

  public override int calculateDamage(Character source, Character target) {
    return (int)((source.attr.intelligence*(1.5f+level*0.1) - target.attr.magicDefense)*(100 - target.attr.fireResistance)/100f);
  }


}

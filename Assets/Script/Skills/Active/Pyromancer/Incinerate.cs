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
    return (int)((source.intelligence*(1.5f+level*0.1) - target.magicDefense)*target.fireResMultiplier);
  }


}

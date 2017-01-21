using UnityEngine;
using System.Collections.Generic;

public class Fireball: SingleTarget {
  public Fireball() {
    range = 3;
    useWepRange = false;
    useLos = false;
    name = "Fireball";
    cooldown = 2;
  }

  public override int calculateDamage(Character source, Character target) {
    return (int)((source.attr.intelligence*(1+level*0.1) - target.attr.magicDefense)*(100 - target.attr.fireResistance)/100f);
  }


}

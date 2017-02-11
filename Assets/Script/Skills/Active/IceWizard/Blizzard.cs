using UnityEngine;
using System.Collections.Generic;

public class Blizzard: CircleAoE {
  public Blizzard() {
    range = 3;
    useWepRange = false;
    aoe = 5;
    useLos = false;
    name = "Blizzard";
    effectsTiles = false;
    maxCooldown = 2;
  }

  public override int calculateDamage(Character source, Character target) {
    return (int)((source.intelligence*(1+level*0.1) - target.magicDefense) * target.iceResMultiplier);
  }
}

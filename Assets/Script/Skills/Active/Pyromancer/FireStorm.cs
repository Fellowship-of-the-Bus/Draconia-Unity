using UnityEngine;
using System.Collections.Generic;

public class FireStorm: CircleAoE {
  public FireStorm() {
    range = 3;
    useWepRange = false;
    aoe = 2;
    useLos = false;
    name = "Fire Storm";
    effectsTiles = false;
    maxCooldown = 2;
  }

  public override int calculateDamage(Character source, Character target) {
    return (int)((source.intelligence*(1+level*0.1) - target.magicDefense)*target.fireResMultiplier);
  }
}

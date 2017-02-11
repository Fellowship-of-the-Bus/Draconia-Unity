using UnityEngine;
using System.Collections.Generic;

public class Volley: CircleAoE {
  public Volley() {
    requireWeapon(Weapon.kinds.Ranged);
    useWepRange = true;
    aoe = 2;
    useLos = false;
    name = "Volley";
    effectsTiles = false;
    maxCooldown = 2;
  }

  public override int calculateDamage(Character source, Character target) {
    return (int)(source.strength*(1+level*0.1) - target.physicalDefense);
  }
}

using UnityEngine;
using System.Collections.Generic;

public class Volley: CircleAoE {

  public override string animation { get { return "Shoot"; }}

  public Volley() {
    requireWeapon(Weapon.Kinds.Ranged);
    useWepRange = true;
    aoe = 2;
    useLos = false;
    name = "Volley";
    effectsTiles = false;
    maxCooldown = 2;
    targetAlly(false);
    targetEnemy(true);
  }

  public override int damageFormula() {
    return (int)(self.strength*(1+level*0.1));
  }
}

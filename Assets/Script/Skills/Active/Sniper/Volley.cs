using UnityEngine;
using System.Collections.Generic;

public class Volley: CircleAoE {
  public override string tooltipDescription { get {
    return "Fire a volley of arrows dealing " + tooltipDamage + " to all characters in the area";
  }}
  public override string animation { get { return "Shoot"; }}

  public Volley() {
    requireWeapon(Weapon.Kind.Ranged);
    useWepRange = true;
    aoe = 2;
    useLos = false;
    name = "Volley";
    effectsTiles = false;
    maxCooldown = 2;
    dType = DamageType.physical;

    targetAlly(false);
    targetEnemy(true);
  }

  public override int damageFormula() {
    return (int)(attributes.strength*(1+level*0.1));
  }
}

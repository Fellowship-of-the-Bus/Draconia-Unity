using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PointBlankShot: SingleTarget {

  public PointBlankShot() {
    requireWeapon(Weapon.kinds.Ranged);
    useWepRange = true;
    useLos = true;
    name = "Point Blank Shot";
    maxCooldown = 2;
  }
  public override int damageFormula() {
    return (int)(self.strength*(1+level*0.1));
  }

  public override int calculateDamage(BattleCharacter target) {
    float distance = (target.gameObject.transform.position - self.gameObject.transform.position).magnitude;
    float multiplier = 2.5f / distance;

    return (int)(base.calculateDamage(target) * multiplier);
  }
}

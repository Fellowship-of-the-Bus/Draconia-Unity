using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PointBlankShot: SingleTarget {

  public PointBlankShot() {
    requireWeapon(Weapon.kinds.Ranged);
    useWepRange = true;
    useLos = true;
    name = "Point Blank Shot";
    cooldown = 2;
  }

  public override int calculateDamage(Character source, Character target) {
    float distance = (target.gameObject.transform.position - source.gameObject.transform.position).magnitude;
    float multiplier = 2.5f / distance;

    return (int)((source.strength*(1+level*0.1) - target.physicalDefense) * multiplier);
  }
}

﻿using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PointBlankShot: RangedTargeting {

  public PointBlankShot() {
    useLos = false;
    name = "Point Blank Shot";
  }

  public override int calculateDamage(Character source, Character target) {
    float distance = (target.gameObject.transform.position - source.gameObject.transform.position).magnitude;
    float multiplier = 2.5f / distance;

    return (int)((source.attr.strength*(1+level*0.1) - target.attr.physicalDefense) * multiplier);
  }
}

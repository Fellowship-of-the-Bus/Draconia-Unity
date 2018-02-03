﻿using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class HookShot: TargetMover {

  public HookShot() {
    requireWeapon(Weapon.Kinds.Ranged);
    useWepRange = true;
    useLos = true;
    name = "Hook Shot";
    maxCooldown = 2;
    targetAlly(false);
    targetEnemy(true);
    setDirection(TargetMover.Direction.towards);
  }

  protected override string tooltipDescription { get {
    return "Deal " + tooltipDamage + " damage and pull the target up to 2 spaces closer";
  }}

  public override int damageFormula() {
    return (int)(self.strength*(1+level*0.1));
  }
}

using UnityEngine;
using System.Collections.Generic;
using System;

public class BluntShot: SingleTarget {
  const int actionReduction = 200;
  public override string animation { get { return "Shoot"; }}

  public BluntShot() {
    requireWeapon(Weapon.Kind.Ranged);
    useWepRange = true;
    useLos = true;
    name = "Blunt Shot";
    maxCooldown = 2;
    dType = DamageType.physical;

    targetAlly(false);
    targetEnemy(true);
  }

  protected override string tooltipDescription { get {
    return "Fire a blunt arrow that deals " + tooltipDamage + " and reduces the target's action by " + actionReduction.ToString();
  }}

  public override int damageFormula() {
    return (int)(attributes.strength*(0.5+level*0.05));
  }

  public override void additionalEffects (BattleCharacter target) {
    target.curAction = Math.Max(0, target.curAction - actionReduction);
    ActionBar.get.updateTime(target);
  }
}

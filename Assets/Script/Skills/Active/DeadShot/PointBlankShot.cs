using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PointBlankShot: SingleTarget {
    float rangeMultiplier = 2.5f;

  public PointBlankShot() {
    requireWeapon(Weapon.Kinds.Ranged);
    useWepRange = true;
    useLos = true;
    name = "Point Blank Shot";
    maxCooldown = 2;
    targetAlly(false);
    targetEnemy(true);
  }
  public override int damageFormula() {
    return (int)(self.strength*(1+level*0.1));
  }

  protected override string tooltipDescription { get {
    return "Deal up to <color=" + tooltipDamageColor + ">" + (int)(damageFormula() * rangeMultiplier)
    + "</color> damage, decreasing against targets further away.";
  }}

  public override int calculateDamage(BattleCharacter target, Tile attackOrigin = null) {
    float distance = (target.transform.position - self.transform.position).magnitude;
    float multiplier = rangeMultiplier / distance;

    return (int)(base.calculateDamage(target, attackOrigin) * multiplier);
  }
}

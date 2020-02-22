using UnityEngine;
using System.Collections.Generic;

public class IceSpear: SingleTarget {

  public override string animation { get { return "Cast"; }}
  public override Color castColor { get { return castColorIceWizard; }}

  public IceSpear() {
    range = 3;
    useWepRange = false;
    useLos = true;
    name = "Ice Spear";
    maxCooldown = 2;

    dType = DamageType.magical;
    dEle = DamageElement.ice;
    targetAlly(false);
    targetEnemy(true);
    projectileType = ProjectileType.IceSpear;
    projectileMoveType = ProjectileMovementType.Straight;
    projectileSpeed = 2f;
  }

  public override string tooltipDescription { get {
    return "Hurl a spear of ice dealing " + tooltipDamage + " damage to the target";
  }}

  public override int damageFormula() {
    return (int)(attributes.intelligence*(1+level*0.1));
  }
}

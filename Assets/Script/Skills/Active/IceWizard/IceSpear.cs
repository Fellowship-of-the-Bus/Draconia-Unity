using UnityEngine;
using System.Collections.Generic;

public class IceSpear: SingleTarget {

  public override string animation { get { return "Cast"; }}

  public IceSpear() {
    range = 3;
    useWepRange = false;
    useLos = false;
    name = "Ice Spear";
    maxCooldown = 2;

    dType = DamageType.magical;
    dEle = DamageElement.ice;
    targetAlly(false);
    targetEnemy(true);
  }

  protected override string tooltipDescription { get {
    return "Hurl a spear of ice dealing " + tooltipDamage + " damage to the target";
  }}

  public override int damageFormula() {
    return (int)(self.intelligence*(1+level*0.1));
  }
}

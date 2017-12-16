using UnityEngine;
using System.Collections.Generic;

public class Blizzard: CircleAoE {
  public Blizzard() {
    range = 3;
    useWepRange = false;
    aoe = 5;
    useLos = false;
    name = "Blizzard";
    effectsTiles = false;
    maxCooldown = 2;
    dType = DamageType.magical;
    dEle = DamageElement.ice;
    targetAlly(false);
    targetEnemy(true);
  }

  public override string tooltip { get { return tooltipHeader
    + "Deal " + tooltipDamage + " damage to all characters in the affected area"; }}

  public override int damageFormula() {
    return (int)(self.intelligence*(1+level*0.1));
  }
}

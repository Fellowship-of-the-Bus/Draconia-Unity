using UnityEngine;
using System.Collections.Generic;

public class Blizzard: CircleAoE {

  public override string animation { get { return "Cast"; }}
  public override Color castColor { get { return castColorIceWizard; }}

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

  protected override string tooltipDescription { get {
    return "Deal " + tooltipDamage + " damage to all characters in the affected area";
  }}

  public override int damageFormula() {
    return (int)(self.intelligence*(1+level*0.1));
  }
}

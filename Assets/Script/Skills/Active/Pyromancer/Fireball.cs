using UnityEngine;
using System.Collections.Generic;

public class Fireball: SingleTarget {

  public override string animation { get { return "Cast"; }}

  public Fireball() {
    range = 3;
    useWepRange = false;
    useLos = false;
    name = "Fireball";
    maxCooldown = 2;

    dType = DamageType.magical;
    dEle = DamageElement.fire;
    targetAlly(false);
    targetEnemy(true);
  }

  protected override string tooltipDescription { get {
    return "Deal " + tooltipDamage + " damage";
  }}

  public override int damageFormula(){
    return (int)(self.intelligence*(1+level*0.1));
  }
}

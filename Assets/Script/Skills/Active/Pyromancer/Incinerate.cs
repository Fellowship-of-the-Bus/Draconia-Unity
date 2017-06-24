using UnityEngine;
using System.Collections.Generic;

public class Incinerate: SingleTarget {
  public Incinerate() {
    range = 3;
    useWepRange = false;
    useLos = false;
    name = "Incinerate";
    maxCooldown = 2;

    dType = DamageType.magical;
    dEle = DamageElement.fire;
    targetAlly(false);
    targetEnemy(true);
  }

  public override int damageFormula() {
    return (int)(self.intelligence*(1.5f+level*0.1));
  }


}

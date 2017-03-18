using UnityEngine;
using System.Collections.Generic;

public class FireStorm: CircleAoE {
  public new bool targetsTiles = true;

  public FireStorm() {
    range = 3;
    useWepRange = false;
    aoe = 2;
    useLos = false;
    name = "Fire Storm";
    effectsTiles = false;
    maxCooldown = 2;

    dType = DamageType.magical;
    dEle = DamageElement.fire;
  }

  public override int damageFormula() {
    return (int)(self.intelligence*(1+level*0.1));
  }
}

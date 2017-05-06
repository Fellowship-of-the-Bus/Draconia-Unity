using UnityEngine;
using System.Collections.Generic;

public class IceSpear: SingleTarget {
  public IceSpear() {
    range = 3;
    useWepRange = false;
    useLos = false;
    name = "Ice Spear";
    maxCooldown = 2;

    dType = DamageType.magical;
    dEle = DamageElement.ice;
  }
  public override List<Tile> getTargets() {
    return getTargetsInRange();
  }

  public override int damageFormula() {
    return (int)(self.intelligence*(1+level*0.1));
  }
}

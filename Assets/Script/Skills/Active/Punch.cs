using UnityEngine;
using System.Collections.Generic;

public class Punch: SingleTarget {
  public Punch() {
    requireMelee();
    useWepRange = true;
    useLos = false;
    name = "Punch";
    maxCooldown = 2;
  }


  public override int damageFormula() {
    return (int)(self.strength*(1+level*0.1));
  }
}

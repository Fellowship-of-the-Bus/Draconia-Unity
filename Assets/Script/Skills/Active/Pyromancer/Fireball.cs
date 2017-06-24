using UnityEngine;
using System.Collections.Generic;

public class Fireball: SingleTarget {
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

  public override int damageFormula(){
    return (int)(self.intelligence*(1+level*0.1));
  }


}

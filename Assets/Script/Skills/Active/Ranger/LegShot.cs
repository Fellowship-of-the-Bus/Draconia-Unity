using UnityEngine;
using System.Collections.Generic;

public class LegShot: SingleTarget {
  public LegShot() {
    requireWeapon(Weapon.kinds.Ranged);
    useWepRange = true;
    useLos = true;
    name = "Leg Shot";
  }

  public override int damageFormula() {
    return (int)(self.strength*(1+level*0.1));
  }

  public override void additionalEffects (BattleCharacter target) {
    CrippleEffect debuff = new CrippleEffect();
    debuff.level = level;
    debuff.duration = 2;
    target.applyEffect(debuff);
  }
}

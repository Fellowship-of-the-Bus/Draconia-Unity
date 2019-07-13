using UnityEngine;
using System.Collections.Generic;

public class LegShot: SingleTarget {

  public override string animation { get { return "Shoot"; }}

  public LegShot() {
    requireWeapon(Weapon.Kinds.Ranged);
    useWepRange = true;
    useLos = true;
    name = "Leg Shot";
    dType = DamageType.physical;

    targetAlly(false);
    targetEnemy(true);
  }

  public override int damageFormula() {
    return (int)(self.strength*(1+level*0.1));
  }

  public override void additionalEffects (BattleCharacter target) {
    CrippleEffect debuff = new CrippleEffect();
    debuff.effectValue = 2;
    debuff.duration = 2;
    debuff.caster = self;
    target.applyEffect(debuff);
  }
}

using UnityEngine;
using System.Collections.Generic;

public class LegShot: SingleTarget {
  const int crippleDuration = 2;
  protected override string tooltipDescription { get {
    return "Shoot the target in the leg dealing " + tooltipDamage + " and crippling them for " + crippleDuration + " turns";
  }}


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
    return (int)(attributes.strength*(1+level*0.1));
  }

  public override void additionalEffects (BattleCharacter target) {
    CrippleEffect debuff = new CrippleEffect();
    debuff.effectValue = 2;
    debuff.duration = crippleDuration;
    debuff.caster = self;
    target.applyEffect(debuff);
  }
}

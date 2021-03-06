using UnityEngine;
using System.Collections.Generic;

public class IgniteWeapon: SingleTarget {

  public override string animation { get { return "Cast"; }}
  public override Color castColor { get { return castColorPyromancer; }}

  public IgniteWeapon() {
    range = 1;
    useLos = false;
    name = "Ignite Weapon";
    maxCooldown = 2;
    canTargetSelf = true;
    targetAlly(true);
    targetEnemy(false);
  }

  public override string tooltipDescription { get {
    return "Imbue the target's weapon with fire";
  }}

  public override void additionalEffects (BattleCharacter target) {
    IgniteWeaponEffect buff = new IgniteWeaponEffect();
    buff.effectValue = level;
    buff.duration = 2;
    buff.caster = self;
    target.applyEffect(buff);
  }
}

using UnityEngine;
using System.Collections.Generic;

public class IgniteWeapon: SingleTarget {

  public override string animation { get { return "Cast"; }}
  public override Color castColor { get { return castColorPyromancer; }}

  public IgniteWeapon() {
    range = 1;
    useLos = false;
    name = "IgniteWeapon";
    maxCooldown = 2;
    canTargetSelf = true;
    targetAlly(true);
    targetEnemy(false);
  }

  protected override string tooltipDescription { get {
    return "Imbue the target's weapon with fire";
  }}

  public override void additionalEffects (BattleCharacter target) {
    IgniteWeaponEffect buff = new IgniteWeaponEffect();
    buff.level = level;
    buff.duration = 2;
    target.applyEffect(buff);
  }
}

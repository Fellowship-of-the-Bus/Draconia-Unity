using UnityEngine;
using System.Collections.Generic;

public class Cripple: SingleTarget {
  public Cripple() {
    requireMelee();
    useWepRange = true;
    useLos = false;
    name = "Cripple";
    maxCooldown = 2;
    targetAlly(false);
    targetEnemy(true);
  }
  public override string tooltip { get { return "Deal " + damageFormula().ToString() + " damage and cripple the target, reducing their movement range by 2"; }}

  public override int damageFormula() {
    return (int)(self.strength*(0.5+level*0.05));
  }

  public override void additionalEffects (BattleCharacter target) {
    CrippleEffect debuff = new CrippleEffect();
    debuff.level = level;
    debuff.duration = 2;
    target.applyEffect(debuff);
  }
}

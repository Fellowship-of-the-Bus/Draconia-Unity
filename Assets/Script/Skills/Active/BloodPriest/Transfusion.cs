using UnityEngine;
using System.Collections.Generic;

public class Transfusion: SingleTarget, HealingSkill {

  public override string animation { get { return "Cast"; }}
  public override Color castColor { get { return castColorBloodPriest; }}

  public Transfusion() {
    range = 3;
    useWepRange = false;
    useLos = false;
    name = "Transfusion";
    maxCooldown = 0;
    targetAlly(true);
    targetEnemy(false);
  }

  public int healingFormula() {
    return (int)(self.intelligence*(1+level*0.1));
  }

  public override void additionalEffects(BattleCharacter target) {
    //This can kill you, should do something about this.
    self.takeDamage(healingFormula(),self);
  }
}

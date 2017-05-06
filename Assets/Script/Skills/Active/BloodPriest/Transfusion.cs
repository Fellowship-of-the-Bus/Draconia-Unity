using UnityEngine;
using System.Collections.Generic;

public class Transfusion: SingleTarget, HealingSkill {
  public Transfusion() {
    range = 3;
    useWepRange = false;
    useLos = false;
    name = "Transfusion";
    maxCooldown = 0;
  }

  public int healingFormula() {
    return (int)(self.intelligence*(1+level*0.1));
  }

  public override void additionalEffects(BattleCharacter target) {
    //This can kill you, should do something about this.
    self.takeDamage(healingFormula());
  }
}

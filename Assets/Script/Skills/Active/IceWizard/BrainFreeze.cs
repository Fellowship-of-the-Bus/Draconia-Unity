using UnityEngine;
using System.Collections.Generic;

public class BrainFreeze: SingleTarget {

  public override string animation { get { return "Cast"; }}

  public BrainFreeze() {
    useWepRange = false;
    useLos = false;
    name = "BrainFreeze";
    maxCooldown = 2;
    range = 3;

    dType = DamageType.magical;
    dEle = DamageElement.ice;

    targetAlly(false);
    targetEnemy(true);
  }

  public override void additionalEffects (BattleCharacter target) {
    foreach (ActiveSkill a in target.equippedSkills) {
      if (a.curCooldown > 0) a.curCooldown += level;
    }
  }
}

using UnityEngine;
using System.Collections.Generic;
using System;

public class FrostArmor: SingleTarget {
  public FrostArmor() {
    range = 3;
    useWepRange = false;
    useLos = false;
    name = "Frost Armor";
    maxCooldown = 2;

    dType = DamageType.magical;
    dEle = DamageElement.ice;
  }

  public override void additionalEffects (BattleCharacter target) {
    FrostArmorEffect debuff = new FrostArmorEffect();
    debuff.level = level;
    debuff.duration = 3;
    target.applyEffect(debuff);
  }

}

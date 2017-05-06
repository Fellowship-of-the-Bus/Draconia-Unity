using System;
using UnityEngine;
using System.Collections.Generic;

public class Disable : SingleTarget {
  public Disable() {
    useWepRange = true;
    useLos = false;
    name = "Disable";
    maxCooldown = 0;
  }
  // TODO: UI to select skill
  public override void additionalEffects (BattleCharacter target) {
    DisableEffect debuff = new DisableEffect();
    debuff.origin = this;
    debuff.target = target.equippedSkills[0]; // TODO: fix this to use the skill that the player selected
    debuff.level = level;
    target.applyEffect(debuff);
  }
}


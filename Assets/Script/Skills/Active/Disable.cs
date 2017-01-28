using System;
using UnityEngine;
using System.Collections.Generic;

public class Disable : SingleTarget {
  public Disable() {
    useWepRange = true;
    useLos = false;
    name = "Disable";
    cooldown = 0;
  }

  public override void additionalEffects (Character target) {
    DisableEffect debuff = new DisableEffect();
    debuff.origin = this;
    debuff.target = target.equippedSkills[0]; // TODO: fix this to use the skill that the player selected
    debuff.level = level;
    target.applyEffect(debuff);
  }
}


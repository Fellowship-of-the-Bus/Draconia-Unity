﻿using UnityEngine;
using System.Collections.Generic;

public class Transfusion: SingleTarget, HealingSkill {
  public Transfusion() {
    range = 3;
    useWepRange = false;
    useLos = false;
    name = "Transfusion";
    cooldown = 0;
  }

  public int calculateHealing(Character source, Character target) {
    source.takeDamage((int)(source.intelligence * (1 + level * 0.1)));
    return (int)(source.intelligence*(1+level*0.1) * target.healingMultiplier);
  }
}

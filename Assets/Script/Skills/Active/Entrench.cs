﻿using UnityEngine;
using System;
using System.Collections.Generic;

public class Entrench: ActiveSkill {
  public int aoe {get; set;}

  public Entrench() {
    range = 0;
    useWepRange = false;
    aoe = 1;
    useLos = false;
    name = "Entrench";
  }

  public override void additionalEffects (Character target) {
    EntrenchEffect buff = new EntrenchEffect();
    buff.level = level;
    buff.duration = -1;
    target.applyEffect(buff);
  }

  public override List<GameObject> getTargets() {
    List<GameObject> targets = new List<GameObject>();
    targets.Add(self.gameObject);
    return targets;
  }

  public override int calculateDamage(Character source, Character target) {
    return 0;
  }
}

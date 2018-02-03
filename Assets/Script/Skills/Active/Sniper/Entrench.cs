using UnityEngine;
using System;
using System.Collections.Generic;

public class Entrench: SelfTarget {
  public int aoe {get; set;}

  public override string animation { get { return "ClericCast"; }}

  public Entrench() {
    range = 0;
    useWepRange = false;
    aoe = 1;
    useLos = false;
    name = "Entrench";
    maxCooldown = 2;
    targetAlly(true);
    targetEnemy(false);
  }

  public override void additionalEffects (BattleCharacter target) {
    EntrenchEffect buff = new EntrenchEffect();
    buff.level = level;
    buff.duration = -1;
    target.applyEffect(buff);
  }
}

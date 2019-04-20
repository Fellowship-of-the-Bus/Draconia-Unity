using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class DamageAll : BFTimeEvent{

  public DamageAll(int t) : base(t){}

  public override void trigger() {
    foreach (BattleCharacter c in GameManager.get.players) {
      c.takeDamage(2);
    }
    foreach (BattleCharacter c in GameManager.get.enemies) {
      c.takeDamage(3);
    }
  }
}

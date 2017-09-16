using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public abstract class BFTurnEvent : BFEvent {
  // be sure to set the boss when you create it.
  BattleCharacter boss;
  int bossTurns = 0;

  //if boss == null, no events get triggered.
  public BFTurnEvent(BattleCharacter c, int t) {
    boss = c;
    triggerTime = t;
  }

  public override void onEvent(Event e) {
    if (boss == null) return;
    if (e.endTurnChar == boss) {
      bossTurns += 1;
      if (bossTurns == triggerTime) {
        trigger();
      }
    }
  }
}

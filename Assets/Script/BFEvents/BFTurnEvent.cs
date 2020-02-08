using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public abstract class BFTurnEvent : BFEvent {
  // be sure to set the boss when you create it, or Brodric will be used by default.
  BattleCharacter boss;
  int bossTurns = 0;

  public override void init() {
    attachListener(GameManager.get.eventManager, EventHook.startTurn);
  }

  //if boss == null, no events get triggered.
  public BFTurnEvent(BattleCharacter c, int t) {
    boss = c;
    triggerTime = t;
  }

  public override void onEvent(Draconia.Event e) {
    if (boss == null) {
      if (e.sender.name != "Brodric") {
        return;
      }
    } else {
      if (e.sender != boss) {
        return;
      }
    }

    bossTurns += 1;
    if (bossTurns == triggerTime) {
      trigger();
    }
  }
}

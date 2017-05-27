using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public abstract class BFTurnEvent : BFEvent {
  // be sure to set the boss when you create it.
  /*BattleCharacter boss;
  int bossTurns = 0;

  public override void onEvent(Event e) {
    if (e.endTurnChar == boss) {
      bossTurns += 1;
      if (bossTurns == triggerTime) {
        trigger();
      }
    }
  }*/
}

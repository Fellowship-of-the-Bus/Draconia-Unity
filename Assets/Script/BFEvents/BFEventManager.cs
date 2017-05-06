using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class BFEventManager : EventListener {
  //Manager class to manage more complex trigger conditions
  public List<BFEvent> timeTriggered = new List<BFEvent>();
  public List<BFEvent> turnTriggered = new List<BFEvent>();
  BattleCharacter boss;
  int previousTime;
  int bossTurns = 0;

  public BFEventManager () {
    attachListener(EventManager.get, EventHook.endTurn);
  }

  public override void onEvent(Event e) {
    BattleCharacter endTurnChar = e.endTurnChar;
    float nextCharTime = e.nextCharTime;

    if (endTurnChar == boss) {
      bossTurns += 1;
    }

    doTimeTriggeredEvents((int)nextCharTime);
    doTurnTriggeredEvents();
  }

  public void setBoss(BattleCharacter c) {
    boss = c;
  }

  void doTimeTriggeredEvents(int newTime) {
    foreach (BFEvent e in timeTriggered) {
      if (previousTime < e.triggerTime && e.triggerTime <= (int) newTime ) {
        e.onTrigger();
      }
    }
    previousTime = (int)newTime;
  }

  void doTurnTriggeredEvents() {
    foreach (BFEvent e in turnTriggered) {
      if (bossTurns == e.triggerTime) {
        e.onTrigger();
      }
    }
  }
}

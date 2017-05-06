using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public abstract class BFEvent : EventListener{
  public int triggerTime;

  public void init() {
    attachListener(EventManager.get, EventHook.endTurn);
  }

  public abstract void trigger();
}

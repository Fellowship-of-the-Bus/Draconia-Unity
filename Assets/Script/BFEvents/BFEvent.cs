using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public abstract class BFEvent : EventListener{
  public int triggerTime;

  public virtual void init() {
    attachListener(GameManager.get.eventManager, EventHook.endTurn);
  }

  public abstract void trigger();
}

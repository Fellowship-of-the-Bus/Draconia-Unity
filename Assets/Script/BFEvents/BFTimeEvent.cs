using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public abstract class BFTimeEvent : BFEvent{
  int previousTime = 0;

  public BFTimeEvent(int t) {
    triggerTime = t;
  }

  public override void onEvent(Draconia.Event e) {
    float newTime = e.nextCharTime;
    if (previousTime < triggerTime && triggerTime <= (int) newTime ) {
      trigger();
    }
    previousTime = (int)newTime;
  }
}

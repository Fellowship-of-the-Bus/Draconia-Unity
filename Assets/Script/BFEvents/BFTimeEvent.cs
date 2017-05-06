using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public abstract class BFTimeEvent : BFEvent{
  int previousTime;

  public override void onEvent(Event e) {
    float newTime = e.nextCharTime;
    if (previousTime < triggerTime && triggerTime <= (int) newTime ) {
      trigger();
    }
    previousTime = (int)newTime;
  }
}

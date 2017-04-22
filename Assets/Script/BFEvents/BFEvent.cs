using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public abstract class BFEvent{
  public int triggerTime;
  public abstract void onTrigger();
}

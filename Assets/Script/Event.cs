using UnityEngine;
using System.Collections.Generic;
using System;

public class Event {
  public MonoBehaviour sender;
  public EventHook hook;
  public bool finishAttack = true;

  public Event(MonoBehaviour sender, EventHook hook) {
    this.sender = sender;
    this.hook = hook;
  }
}

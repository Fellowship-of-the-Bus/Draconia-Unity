using UnityEngine;
using System.Collections.Generic;
using System;

public class Event {
  public MonoBehaviour sender;
  public EventHook hook;
  public Event(MonoBehaviour sender, EventHook hook) {
    this.sender = sender;
    this.hook = hook;
  }
}

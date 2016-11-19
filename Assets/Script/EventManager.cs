using UnityEngine;
using System.Collections.Generic;
using System;

public enum EventHook {
  preAttack,
  postAttack,
  preMove,
  postMove,
  dodge,
}

public class EventManager {
  Dictionary<EventHook, HashSet<EventListener>> listeners = new Dictionary<EventHook, HashSet<EventListener>>();
  public EventManager() {
    get = this;
    foreach (EventHook i in Enum.GetValues(typeof(EventHook))) {
      listeners.Add(i, new HashSet<EventListener>());
    }
  }

  public void addListener(EventListener listener, EventHook hook) {
    listeners[hook].Add(listener);
  }

  public void removeListener(EventListener listener){
    foreach (EventHook i in Enum.GetValues(typeof(EventHook))) {
      listeners[i].Remove(listener);
    }
  }

  public void onEvent(Event e) {
    foreach (EventListener listener in listeners[e.hook]) {
      listener.onEvent(e);
    }
  }

  public static EventManager get { get; private set; }
}

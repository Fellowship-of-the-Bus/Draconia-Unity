using UnityEngine;
using System.Collections.Generic;
using System;

public enum EventHook {
  onAttack,
  preMove,
  postMove
}

public class EventManager {
  Dictionary<EventHook, HashSet<EventListener>> listeners = new Dictionary<EventHook, HashSet<EventListener>>();
  public EventManager() {
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

  public void onEvent(EventHook hook) {
    foreach (EventListener listener in listeners[hook]) {
      listener.onEvent(hook);
    }
  }
}
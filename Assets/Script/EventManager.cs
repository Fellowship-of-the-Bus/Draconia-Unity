using UnityEngine;
using System.Collections.Generic;
using System;

public enum EventHook {
  preAttack,
  postAttack,
  preDamage,
  postDamage,
  preDeath,
  postDeath,
  preMove,
  postMove,
  dodge,
  endTurn,
}

public class EventManager : MonoBehaviour {
  Queue<Event> eventQueue;
  Dictionary<EventHook, HashSet<EventListener>> listeners = new Dictionary<EventHook, HashSet<EventListener>>();
  protected void Start() {
    get = this;
    foreach (EventHook i in Enum.GetValues(typeof(EventHook))) {
      listeners.Add(i, new HashSet<EventListener>());
    }
    eventQueue = new Queue<Event>();
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
    //if there are events already, its already being dequeued
    if (eventQueue.Count != 0) {
      eventQueue.Enqueue(e);
      return;
    }
    eventQueue.Enqueue(e);
    while (eventQueue.Count != 0) {
      e = eventQueue.Dequeue();
      foreach (EventListener listener in listeners[e.hook]) {
        listener.onEvent(e);
      }

      listeners[e.hook] = new HashSet<EventListener>(listeners[e.hook].Filter( (EventListener listener) => {
        Effect effect = listener as Effect;
        if (effect != null && effect.duration == 0) {
          effect.onDeactivate();
          return false;
        }
        return true;
      }));
    }
  }

  public static EventManager get { get; private set; }
}

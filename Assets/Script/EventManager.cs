using UnityEngine;
using System.Collections.Generic;
using System;

public enum EventHook {
  preAttack, postAttack,
  preDamage, postDamage,
  preDeath, postDeath,
  preMove, postMove, enterTile,
  dodge,
  endTurn, startTurn,
  preHealing, postHealing,
  preHealed, postHealed,
  cancel,
  activateEffect, deactivateEffect,
}

public class EventManager : MonoBehaviour {
  Queue<Event> eventQueue;
  Dictionary<EventHook, HashSet<EventListener>> listeners = new Dictionary<EventHook, HashSet<EventListener>>();

  void Awake() {
    foreach (EventHook i in Enum.GetValues(typeof(EventHook))) {
      listeners.Add(i, new HashSet<EventListener>());
    }
    eventQueue = new Queue<Event>();
  }

  public void setGlobal() {
    get = this;
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

      if (e.hook == EventHook.endTurn) {
        List<EventListener> list = new List<EventListener>(listeners[e.hook]);
        foreach (EventListener listener in list) {
          DurationEffect effect = listener as DurationEffect;
          if (effect != null && effect.duration == 0) {
            if (effect.owner != null) effect.owner.removeEffect(effect);
            if (effect.ownerTile != null) effect.ownerTile.removeEffect(effect);
          }
        }
      }
    }
  }

  public static EventManager get { get; private set; }
}

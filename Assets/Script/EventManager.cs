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
  useBuffSkill, //for enhancer apply same buff to self when buffing others
  preSkill, postSkill,

  NUM_EVENT_HOOKS // Must be last
}

public class EventManager : MonoBehaviour {
  private Queue<Draconia.Event> eventQueue;
  private HashSet<EventListener>[] listeners = new HashSet<EventListener>[(int)EventHook.NUM_EVENT_HOOKS];

  protected void Awake() {
    for (int i = 0; i < (int)EventHook.NUM_EVENT_HOOKS; i++) {
      listeners[i] = new HashSet<EventListener>();
    }
    eventQueue = new Queue<Draconia.Event>();
  }

  public void addListener(EventListener listener, EventHook hook) {
    listeners[(int)hook].Add(listener);
  }

  public void removeListener(EventListener listener){
    for (int i = 0; i < (int)EventHook.NUM_EVENT_HOOKS; ++i) {
      listeners[i].Remove(listener);
    }
  }

  public void onEvent(Draconia.Event e) {
    //if there are events already, its already being dequeued
    if (eventQueue.Count != 0) {
      eventQueue.Enqueue(e);
      return;
    }
    eventQueue.Enqueue(e);
    while (eventQueue.Count != 0) {
      e = eventQueue.Dequeue();
      List<EventListener> list = new List<EventListener>(listeners[(int)e.hook]);

      foreach (EventListener listener in list) {
        listener.onEvent(e);
      }

      // remove expired DurationEffects
      if (e.hook == EventHook.endTurn) {
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

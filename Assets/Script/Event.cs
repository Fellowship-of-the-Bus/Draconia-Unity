using UnityEngine;
using System.Collections.Generic;
using System;

public class Event {
  public Character sender;
  public EventHook hook;
  public bool finishAttack = true;
  public bool preventDeath = false;
  public Character newTarget;

  public int damageTaken;
  public Character attackTarget;
  public ActiveSkill skillUsed;

  public Character endTurnChar;

  public Vector3 position;

  public Event(Character sender, EventHook hook) {
    this.sender = sender;
    this.hook = hook;
  }
}

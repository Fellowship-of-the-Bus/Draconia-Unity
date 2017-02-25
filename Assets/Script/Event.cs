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

  public int healingDone;
  public Character healTarget;

  public Character endTurnChar;

  public Vector3 position;

  public Effect appliedBuff;

  public List<Effected> targets;

  public Event(Effected sender, EventHook hook) {
    this.sender = sender as Character;
    this.hook = hook;
  }
}

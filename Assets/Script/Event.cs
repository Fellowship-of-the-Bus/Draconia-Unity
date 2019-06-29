using UnityEngine;
using System.Collections.Generic;
using System;

namespace Draconia {
  public class Event {
    public BattleCharacter sender;
    public EventHook hook;
    public bool finishAttack = true;
    public bool preventDeath = false;
    public BattleCharacter newTarget;

    //Post death
    public BattleCharacter killer;

    public int damageTaken;
    public BattleCharacter attackTarget;
    public ActiveSkill skillUsed;

    public int healingDone;
    public BattleCharacter healTarget;

    public BattleCharacter endTurnChar;
    public float nextCharTime;

    public Vector3 position;

    public Effect appliedBuff;

    public List<Effected> targets;

    public bool interruptMove = false;


    public Event(Effected sender, EventHook hook) {
      this.sender = sender as BattleCharacter;
      this.hook = hook;
    }
  }
}


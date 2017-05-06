using UnityEngine;
using System.Collections.Generic;

public abstract class PassiveSkill : Effect, Skill {
  int Skill.level { get { return level; } set { level = value; } }

  public BattleCharacter self {get; set;}
  public int Range {get; set;}
  public bool useLos {get; set;}
  public string name {get; set;}

  public void activate(BattleCharacter target) {
    target.applyEffect(this);
  }
}

using UnityEngine;
using System.Collections.Generic;

public abstract class PassiveSkill : Effect, Skill {
  int Skill.level { get { return level; } set { level = value; } }

  public BattleCharacter self {get; set;}
  public int range {get; set;}
  public bool useLos {get; set;}

  public void activate(BattleCharacter target) {
    this.caster = self;
    target.applyEffect(this);
  }
}

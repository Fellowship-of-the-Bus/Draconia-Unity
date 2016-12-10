using UnityEngine;
using System.Collections.Generic;

public abstract class PassiveSkill : Effect, Skill {
  int Skill.level { get { return level; } set { level = value; } }

  public Character self {get; set;}
  public int range {get; set;}
  public bool useLos {get; set;}
  public string name {get; set;}

  public void activate(Character target) {
    target.applyEffect(this);
  }

  public abstract List<GameObject> getTargets();
}

using UnityEngine;
using System.Collections.Generic;

public abstract class ActiveSkill : Skill {
  public int level {get; set;}
  public Character self {get; set;}
  public int range {get; set;}
  public bool useLos {get; set;}
  public string name {get; set;}
  public int cooldown {get; set;}

  public virtual void activate(Character target) {
    target.takeDamage(calculateDamage(self, target));
    additionalEffects(target);
  }
  public abstract int calculateDamage(Character source, Character target);
  public virtual void additionalEffects(Character target) {

  }

  public abstract List<GameObject> getTargets();
}

using UnityEngine;
using System.Collections.Generic;

public abstract class ActiveSkill : EventListener, Skill {
  public int level {get; set;}
  public Character self {get; set;}
  public int range {get; set;}
  public bool useWepRange {get; set;}
  public bool useLos {get; set;}
  public string name {get; set;}
  public int cooldown {get; set;}
  //number of turns before usable
  int curCooldown = 0;

  public virtual void activate(Character target) {
    target.takeDamage(calculateDamage(self, target));
    additionalEffects(target);
  }
  public abstract int calculateDamage(Character source, Character target);
  public virtual void additionalEffects(Character target) {

  }

  public abstract List<GameObject> getTargets();

  public virtual bool canUse() {
    return curCooldown == 0;
  }

  bool attachedListener = false;
  public virtual void setCooldown() {
    if (!attachedListener) {
      attachListener(self, EventHook.endTurn);
      attachedListener = true;
    }
    curCooldown = cooldown;
  }

  public override void onEvent(Event e) {
    if (curCooldown != 0) {
      curCooldown -= 1;
    }
  }
}

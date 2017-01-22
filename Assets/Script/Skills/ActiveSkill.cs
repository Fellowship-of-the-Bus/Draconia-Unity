using UnityEngine;
using System.Collections.Generic;

public abstract class ActiveSkill : EventListener, Skill {
  public const int InfiniteCooldown = -2;

  public int level {get; set;}
  public Character self {get; set;}
  public int range {get; set;}
  public bool useWepRange {get; set;}
  public bool useLos {get; set;}
  public string name {get; set;}
  public int cooldown {get; set;}
  //number of turns before usable
  public int curCooldown = 0;

  public virtual void activate(Character target) {
    if (this is HealingSkill) {
      HealingSkill heal = this as HealingSkill;
      target.takeHealing(heal.calculateHealing(self,target));
    } else {
      target.takeDamage(calculateDamage(self, target));
    }
    additionalEffects(target);
  }

  public virtual void activate(Tile target) {
    tileEffects(target);
  }
  public virtual int calculateDamage(Character source, Character target) {
    return 0;
  }
  public virtual void additionalEffects(Character target) {

  }

  public virtual void tileEffects(Tile target) {

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
    curCooldown = cooldown + 1;
  }

  public override void onEvent(Event e) {
    if (curCooldown != 0) {
      curCooldown -= 1;
    }
  }
}

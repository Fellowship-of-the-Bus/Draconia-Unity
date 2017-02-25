using UnityEngine;
using System.Collections.Generic;

public class BloodJudgement: CircleAoE {
  bool listenerAttached = false;
  int cost = 0;

  public BloodJudgement() {
    range = 3;
    useWepRange = false;
    aoe = 2;
    useLos = false;
    name = "Blood Judgement";
    effectsTiles = false;
    maxCooldown = 2;
    targetsTiles = true;
  }

  float percent() {
    if (self.curHealth <= cost) {
      return 1 - ((cost - self.curHealth + 1 * 1.0f) / cost);
    }
    return 1f;
  }
  //This is not the right place to do this, but there doesn't seem to be a better function for it.
  public override void setCooldown() {
    base.setCooldown();
    if (!listenerAttached) {
      attachListener(self, EventHook.postSkill);
      listenerAttached = true;
      cost = self.maxHealth / 10;
    }
  }

  public override void onEvent(Event e) {
    base.onEvent(e);
    if (e.hook == EventHook.postSkill) {
      self.takeDamage((int)(cost * percent()));
    }
  }

  int amount() {
    return (int)(self.intelligence * 0.25f);
  }

  public override void additionalEffects (Character target) {
    if (target == self) return;

    if (target.team == self.team) {
      target.takeHealing((int)(amount() * percent()));
    } else {
      target.takeDamage((int)(amount() * percent()));
    }
  }

    public override List<GameObject> getTargetsInAoe(Vector3 position) {
    List<GameObject> l = base.getTargetsInAoe(position);
    l.Remove(self.gameObject);
    return l;
  }
}

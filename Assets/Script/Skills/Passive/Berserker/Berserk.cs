using UnityEngine;
using System.Collections.Generic;
using System;

public class Berserk : PassiveSkill {
  public override List<GameObject> getTargets() {
    List<GameObject> targets = new List<GameObject>();
    targets.Add(self.gameObject);
    return targets;
  }

  protected override void onActivate() {
    attachListener(owner, EventHook.postDamage);
  }
  protected override void onDeactivate() {
    detachListener(owner);
  }
  protected override void additionalEffect(Event e) {
    double percentage = Math.Abs(e.damageTaken)/(double)owner.maxHealth;

    if (percentage == 0) {
      return;
    }
    percentage = percentage * (1 + (level -1));
    owner.curAction = (float)Math.Min(owner.curAction + owner.maxAction*percentage, owner.maxAction);
    ActionQueue.get.updateTime(owner.gameObject);
  }
}

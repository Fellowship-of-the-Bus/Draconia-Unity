using UnityEngine;
using System;

public class BerserkEffect : Effect {
  int preAttackHP;
  public override void onRemove() {

  }
  public override void onActivate() {
    attachListener(owner, EventHook.preDamage);
    attachListener(owner, EventHook.postDamage);
  }
  public override void onDeactivate() {
    detachListener(owner);
  }
  public override void onEvent(Event e) {
    if (e.hook == EventHook.preDamage) {
      preAttackHP = owner.curHealth;
      return;
    } else if (e.hook == EventHook.postDamage) {
      int postAttackHP = owner.curHealth;
      double percentage = Math.Abs(preAttackHP - postAttackHP)/(double)owner.attr.maxHealth;

      if (percentage == 0) {
        return;
      }
      percentage = percentage * (1 + (level -1));
      owner.curAction = (float)Math.Min(owner.curAction + owner.maxAction*percentage, owner.maxAction);
      ActionQueue.get.updateTime(owner.gameObject);
    }
  }
}

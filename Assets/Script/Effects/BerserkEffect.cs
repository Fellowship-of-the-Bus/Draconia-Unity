using UnityEngine;
using System;

public class BerserkEffect : Effect {
  int preAttackHP;
  public override void onRemove() {

  }
  public override void onActivate() {
    attachListener(EventHook.preAttack);
    attachListener(EventHook.postAttack);
  }
  public override void onDeactivate() {
    detachListener();
  }
  public override void onEvent(Event e) {
    if (e.hook == EventHook.preAttack) {
      preAttackHP = owner.curHealth;
      return;
    } else if (e.hook == EventHook.postAttack) {
      int postAttackHP = owner.curHealth;
      double percentage = Math.Abs(preAttackHP - postAttackHP)/(double)owner.attr.maxHealth;
      percentage = percentage * (1 + (level -1));
      owner.curAction = (float)Math.Min(owner.curAction + owner.maxAction*percentage, owner.maxAction);
      ActionQueue.get.updateTime(owner.gameObject);
    }
  }
}

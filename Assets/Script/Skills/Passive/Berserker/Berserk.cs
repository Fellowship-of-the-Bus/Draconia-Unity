using UnityEngine;
using System.Collections.Generic;
using System;

public class Berserk : PassiveSkill {
  protected override void onActivate() {
    attachListener(owner, EventHook.postDamage);
  }
  protected override void onDeactivate() {
    detachListener(owner);
  }
  protected override void additionalEffect(Draconia.Event e) {
    double percentage = Math.Abs(e.damageTaken)/(double)owner.maxHealth;

    if (percentage == 0) {
      return;
    }
    percentage = percentage * (1 + (level -1));
    owner.curAction = (float)Math.Min(owner.curAction + BattleCharacter.maxAction*percentage, BattleCharacter.maxAction);
    ActionQueue.get.updateTime(owner.gameObject);
  }
}

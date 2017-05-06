using UnityEngine;
using System.Collections.Generic;
using System;

public class Alacrity : PassiveSkill {
  protected override void onActivate() {
    attachListener(owner, EventHook.postHealing);
  }
  protected override void onDeactivate() {
    detachListener(owner);
  }
  protected override void additionalEffect(Event e) {
    if (e.healingDone > 0) {
      BattleCharacter target = e.healTarget;
      target.curAction = Math.Min(target.maxAction, target.curAction + 100f);
      ActionQueue.get.updateTime(target.gameObject);
    }
  }
}

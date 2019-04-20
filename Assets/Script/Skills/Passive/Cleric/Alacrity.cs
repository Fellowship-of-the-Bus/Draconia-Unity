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
  protected override void additionalEffect(Draconia.Event e) {
    if (e.healingDone > 0) {
      BattleCharacter target = e.healTarget;
      target.curAction = Math.Min(BattleCharacter.maxAction, target.curAction + 100f);
      ActionQueue.get.updateTime(target);
    }
  }
}

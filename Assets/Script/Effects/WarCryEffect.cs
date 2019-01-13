using UnityEngine;
using System;

public class WarCryEffect : DurationEffect {
  int amount = 200;
  protected override void onActivate() {
    owner.attrChange.speed += 1;
    owner.curAction = (float)Math.Min(owner.curAction + (amount * level), BattleCharacter.maxAction);
    ActionQueue.get.updateTime(owner.gameObject);
  }
  protected override void onDeactivateEffects() {
    owner.attrChange.speed -= 1;
  }
}

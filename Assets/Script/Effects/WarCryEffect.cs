using UnityEngine;
using System;

public class WarCryEffect : DurationEffect {
  int amount = 200;
  public int scaling;
  protected override void onActivate() {
    owner.attrChange.speed += effectValue;
    owner.curAction = (float)Math.Min(owner.curAction + (amount * scaling), BattleCharacter.maxAction);
    ActionQueue.get.updateTime(owner);
  }
  protected override void onDeactivateEffects() {
    owner.attrChange.speed -= effectValue;
  }
}

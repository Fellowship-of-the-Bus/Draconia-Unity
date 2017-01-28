using UnityEngine;
using System;

public class WarCryEffect : DurationEffect {
  int amount = 200;
  public override void onActivate() {
    owner.attrChange.speed += 1;
    owner.curAction = (float)Math.Min(owner.curAction + (amount * level), owner.maxAction);
    ActionQueue.get.updateTime(owner.gameObject);
  }
  public override void onDeactivateEffects() {
    owner.attrChange.speed -= 1;
  }
}

using UnityEngine;
using System;

public class WarCryEffect : Effect {
  int amount = 200;
  public override void onActivate() {
    owner.attr.speed += 1;
    owner.curAction = (float)Math.Min(owner.curAction + (amount * level), owner.maxAction);
    ActionQueue.get.updateTime(owner.gameObject);
  }
  public override void onDeactivate() {
    owner.attr.speed -= 1;
  }
}

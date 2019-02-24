using System;
using System.Collections.Generic;
using UnityEngine;

public class CounterSkill : PassiveSkill {
  protected override void onActivate() {
    attachListener(owner, EventHook.postDamage);
  }
  protected override void onDeactivate() {
    detachListener(owner);
  }

  protected override void additionalEffect(Draconia.Event e) {
    // don't counter yourself or your teammates.
    if (owner == e.sender || owner.team == e.sender.team) return;
    float chance = UnityEngine.Random.value;
    if (chance < 0.1*level && owner.inRange(e.sender, 1)) {
      Punch atk = new Punch();
      atk.level = level;
      atk.self = owner;

      List<Tile> target = new List<Tile>();
      target.Add(e.sender.curTile);
      owner.useSkill(atk, new List<Tile>(target));
    }
  }
}

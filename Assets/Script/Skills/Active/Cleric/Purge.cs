using UnityEngine;
using System.Collections.Generic;

public class Purge: SingleTarget {
  public Purge() {
    useWepRange = false;
    range = 3;
    useLos = false;
    name = "Purge";
    maxCooldown = 2;
    canTargetSelf = true;
    targetAlly(true);
    targetEnemy(true);
  }

  public override void additionalEffects (BattleCharacter target) {
    foreach (LinkedListNode<Effect> n in new NodeIterator<Effect>(target.allEffects)) {
      if (n.Value is DurationEffect) target.removeEffect(n.Value);
    }
  }
}

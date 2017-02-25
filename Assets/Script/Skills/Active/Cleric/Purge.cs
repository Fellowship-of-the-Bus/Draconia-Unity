using UnityEngine;
using System.Collections.Generic;

public class Purge: SingleTarget {
  public Purge() {
    useWepRange = false;
    range = 3;
    useLos = false;
    name = "Purge";
    maxCooldown = 2;
  }

  public override int calculateDamage(Character source, Character target) {
    return 0;
  }

  public override void additionalEffects (Character target) {
    foreach (LinkedListNode<Effect> n in new NodeIterator<Effect>(target.allEffects)) {
      if (n.Value is DurationEffect) target.removeEffect(n.Value);
    }
  }
}
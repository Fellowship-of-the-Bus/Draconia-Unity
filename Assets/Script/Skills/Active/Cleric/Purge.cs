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
    LinkedList<Effect> toRemove = new LinkedList<Effect>();
    foreach(Effect e in target.allEffects) {
      if (e is DurationEffect) toRemove.AddLast(e);
    }
    foreach(Effect e in toRemove) {
      target.removeEffect(e);
    }
  }
}

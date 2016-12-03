using UnityEngine;
using System.Collections.Generic;

public class KillingBlow: ActiveSkill {
  public KillingBlow() {
    range = 1;
    useLos = false;
    name = "Killing Blow";
  }

  public override List<GameObject> getTargets() {
    List<Tile> tiles = GameManager.get.getTilesWithinRange(self.curTile, 1);
    List<GameObject> targets = new List<GameObject>();
    foreach (Tile t in tiles) {
      if (t.occupied()) {
        targets.Add(t.occupant);
      }
    }
    return targets;
  }

  public override int calculateDamage(Character source, Character target) {
    float missingPct = 1 - (float)target.curHealth/target.attr.maxHealth;

    return (int)((source.attr.strength*(1+level*0.1) - target.attr.physicalDefense) * (1 + missingPct * (0.5f + 0.1f * level)));
  }


}

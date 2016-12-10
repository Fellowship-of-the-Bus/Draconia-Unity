using UnityEngine;
using System.Collections.Generic;

public class Heal: HealingSkill {
  public Heal() {
    range = 3;
    useLos = false;
    name = "Heal";
  }
  public override List<GameObject> getTargets() {
    GameManager gm = GameManager.get;
    List<Tile> tiles = gm.getTilesWithinRange(self.curTile, range);
    List<GameObject> targets = new List<GameObject>();
    foreach (Tile t in tiles) {
      if (t.occupied()) {
        targets.Add(t.occupant);
      }
    }
    return targets;
  }

  public override int calculateHealing(Character source, Character target) {
    return (int)(source.attr.intelligence*(1+level*0.1));
  }


}

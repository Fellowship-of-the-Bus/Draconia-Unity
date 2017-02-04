using UnityEngine;
using System.Collections.Generic;

public class IceSpear: SingleTarget {
  public IceSpear() {
    range = 3;
    useWepRange = false;
    useLos = false;
    name = "Ice Spear";
    maxCooldown = 2;
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


  public override int calculateDamage(Character source, Character target) {
    return (int)((source.intelligence*(1+level*0.1) - target.magicDefense) * target.iceResMultiplier);
  }
}

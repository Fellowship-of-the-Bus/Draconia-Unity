using UnityEngine;
using System.Collections.Generic;

public class IceSpear: ActiveSkill {
  public IceSpear() {
    range = 3;
    useLos = false;
    name = "Ice Spear";
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
    return (int)((source.attr.intelligence*(1+level*0.1) - target.attr.magicDefense) * (100 - target.attr.iceResistance)/100f);
  }
}

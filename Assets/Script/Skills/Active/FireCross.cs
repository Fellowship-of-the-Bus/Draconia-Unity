using UnityEngine;
using System.Collections.Generic;

public class FireCross: ActiveSkill, AoeSkill {
  public int aoe {get; set;}

  public FireCross() {
    range = 4;
    useWepRange = false;
    aoe = 2;
    useLos = false;
    name = "FireCross";
    cooldown = 2;
  }

  public override List<GameObject> getTargets() {
    GameManager gm = GameManager.get;
    List<Tile> tiles = gm.getTilesWithinRange(self.curTile, range);
    List<GameObject> targets = new List<GameObject>();
    foreach (Tile t in tiles) {
      if (t == self.curTile) {
        continue;
      }
      targets.Add(t.gameObject);
    }
    targets.Add(self.curTile.gameObject);
    return targets;
  }

  public List<GameObject> getTargetsInAoe(Vector3 position) {
    GameManager gm = GameManager.get;
    List<Tile> tiles = gm.getCardinalTilesWithinRange(gm.getTile(position), aoe);
    tiles.Add(gm.getTile(position));
    return getObjectsFromTile(tiles);
  }

  List<GameObject> getObjectsFromTile(List<Tile> tiles) {
    List<GameObject> targets = new List<GameObject>();
    foreach (Tile t in tiles) {
        if (t.occupant) targets.Add(t.occupant);
        else targets.Add(t.gameObject);
    }
    return targets;
  }


  public override int calculateDamage(Character source, Character target) {
    return (int)((source.attr.intelligence*(1+level*0.1) - target.attr.magicDefense)*(100 - target.attr.fireResistance)/100f);
  }


}

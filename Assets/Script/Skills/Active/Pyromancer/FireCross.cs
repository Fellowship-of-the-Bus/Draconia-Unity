using UnityEngine;
using System.Collections.Generic;

public class FireCross: ActiveSkill, AoeSkill {
  public int aoe {get; set;}
  public bool effectsTiles {get; set;}

  public FireCross() {
    range = 4;
    useWepRange = false;
    aoe = 2;
    useLos = false;
    name = "FireCross";
    effectsTiles = false;
    maxCooldown = 2;
    targetsTiles = true;

    dType = DamageType.magical;
    dEle = DamageElement.fire;
  }

  public override List<GameObject> getTargets() {
    Map map = GameManager.get.map;
    List<Tile> tiles = map.getTilesWithinRange(self.curTile, range);
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
    Map map = GameManager.get.map;
    List<Tile> tiles = map.getCardinalTilesWithinRange(map.getTile(position), aoe);
    tiles.Add(map.getTile(position));
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


  public override int damageFormula() {
    return (int)(self.intelligence*(1+level*0.1));
  }


}

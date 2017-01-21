using UnityEngine;
using System.Collections.Generic;

public class FireLance: ActiveSkill, AoeSkill {
  public int aoe {get; set;}

  public FireLance() {
    range = 4;
    useWepRange = false;
    aoe = 4;
    useLos = false;
    name = "FireLance";
  }

  public override List<GameObject> getTargets() {
    GameManager gm = GameManager.get;
    List<Tile> tiles = gm.getCardinalTilesWithinRange(self.curTile, range);
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
    List<Tile> tiles = gm.getCardinalTilesWithinRange(self.curTile, aoe);
    var myPosition = self.curTile.gameObject.transform.position;

    //break up the tiles into the 4 cardinal directions
    List<Tile> up = new List<Tile>(tiles.Filter((tile) => tile.gameObject.transform.position.z > myPosition.z + 0.05f));
    List<Tile> down = new List<Tile>(tiles.Filter((tile) => tile.gameObject.transform.position.z < myPosition.z - 0.05f));
    List<Tile> left = new List<Tile>(tiles.Filter((tile) => tile.gameObject.transform.position.x < myPosition.x - 0.05f));
    List<Tile> right = new List<Tile>(tiles.Filter((tile) => tile.gameObject.transform.position.x > myPosition.x + 0.05f));

    Tile t = gm.getTile(position);
    //return the stuff in the right direction
    if (up.Contains(t)) {
      return getObjectsFromTile(up);
    } else if (down.Contains(t)) {
      return getObjectsFromTile(down);
    } else if (left.Contains(t)) {
      return getObjectsFromTile(left);
    } else if (right.Contains(t)) {
      return getObjectsFromTile(right);
    } else {
      return null;
    }
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

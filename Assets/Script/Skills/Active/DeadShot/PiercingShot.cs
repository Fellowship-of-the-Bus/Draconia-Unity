using UnityEngine;
using System.Collections.Generic;

public class PiercingShot: ActiveSkill, AoeSkill {
  public int aoe {get; set;}
  public bool effectsTiles {get; set;}

  public PiercingShot() {
    useLos = false;
    name = "Piercing Shot";
    effectsTiles = false;
    maxCooldown = 2;
    targetAlly(false);
    targetEnemy(true);
  }

  public override List<Tile> getTargets() {
    return getTargetsInRange();
  }

  public List<Tile> getTargetsInAoe(Vector3 position) {
    GameManager gm = GameManager.get;
    List<Tile> tiles = gm.map.getCardinalTilesWithinRange(self.curTile, aoe);
    var myPosition = self.curTile.gameObject.transform.position;

    //break up the tiles into the 4 cardinal directions
    List<Tile> up = new List<Tile>(tiles.Filter((tile) => tile.gameObject.transform.position.z > myPosition.z + 0.05f));
    List<Tile> down = new List<Tile>(tiles.Filter((tile) => tile.gameObject.transform.position.z < myPosition.z - 0.05f));
    List<Tile> left = new List<Tile>(tiles.Filter((tile) => tile.gameObject.transform.position.x < myPosition.x - 0.05f));
    List<Tile> right = new List<Tile>(tiles.Filter((tile) => tile.gameObject.transform.position.x > myPosition.x + 0.05f));

    Tile t = gm.map.getTile(position);
    //return the stuff in the right direction
    if (up.Contains(t)) {
      return up;
    } else if (down.Contains(t)) {
      return down;
    } else if (left.Contains(t)) {
      return left;
    } else if (right.Contains(t)) {
      return right;
    } else {
      return null;
    }
  }

  public override int damageFormula() {
    return (int)(self.strength*(1+level*0.1));
  }
}

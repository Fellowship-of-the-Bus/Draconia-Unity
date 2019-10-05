using UnityEngine;
using System.Collections.Generic;

public class PiercingShot: ActiveSkill, AoeSkill {
  public int aoe {get; set;}
  public bool effectsTiles {get; set;}

  public override string animation { get { return "Shoot"; }}

  public PiercingShot() {
    useLos = false;
    name = "Piercing Shot";
    effectsTiles = false;
    maxCooldown = 2;
    dType = DamageType.physical;

    targetAlly(false);
    targetEnemy(true);
  }

  public List<Tile> getTargetsInAoe(Vector3 position) {
    GameManager gm = GameManager.get;
    List<Tile> tiles = gm.map.getCardinalTilesWithinRange(self.curTile, aoe);
    var myPosition = self.curTile.transform.position;

    //break up the tiles into the 4 cardinal directions
    List<Tile> up = new List<Tile>(tiles.Filter((tile) => tile.transform.position.z > myPosition.z + 0.05f));
    List<Tile> down = new List<Tile>(tiles.Filter((tile) => tile.transform.position.z < myPosition.z - 0.05f));
    List<Tile> left = new List<Tile>(tiles.Filter((tile) => tile.transform.position.x < myPosition.x - 0.05f));
    List<Tile> right = new List<Tile>(tiles.Filter((tile) => tile.transform.position.x > myPosition.x + 0.05f));

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

  protected override string tooltipDescription { get {
    return "Deal " + tooltipDamage + " damage to all targets in a straight line";
  }}

  public override int damageFormula() {
    return (int)(attributes.strength*(1+level*0.1));
  }
}

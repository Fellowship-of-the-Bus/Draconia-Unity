using UnityEngine;
using System.Collections.Generic;

public class FireLance: ActiveSkill, AoeSkill {
  public int aoe {get; set;}
  public bool effectsTiles {get; set;}

  public override string animation { get { return "Cast"; }}
  public override Color castColor { get { return castColorPyromancer; }}

  public FireLance() {
    range = 4;
    useWepRange = false;
    aoe = 4;
    useLos = false;
    name = "Fire Lance";
    effectsTiles = false;
    maxCooldown = 2;
    targetsTiles = true;

    dType = DamageType.magical;
    dEle = DamageElement.fire;
    targetAlly(false);
    targetEnemy(true);

    projectileType = ProjectileType.FireLance;
    projectileMoveType = ProjectileMovementType.Straight;
    projectileSpeed = 3f;
  }

  public override string tooltipDescription { get {
    return "Deal " + tooltipDamage + " damage to all targets in a straight line";
  }}

  public List<Tile> getTargetsInAoe(Vector3 position) {
    Map map = GameManager.get.map;
    List<Tile> tiles = map.getCardinalTilesWithinRange(self.curTile, aoe);
    var myPosition = self.curTile.transform.position;

    //break up the tiles into the 4 cardinal directions
    List<Tile> up = new List<Tile>(tiles.Filter((tile) => tile.transform.position.z > myPosition.z + 0.05f));
    List<Tile> down = new List<Tile>(tiles.Filter((tile) => tile.transform.position.z < myPosition.z - 0.05f));
    List<Tile> left = new List<Tile>(tiles.Filter((tile) => tile.transform.position.x < myPosition.x - 0.05f));
    List<Tile> right = new List<Tile>(tiles.Filter((tile) => tile.transform.position.x > myPosition.x + 0.05f));

    Tile t = map.getTile(position);
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
      return new List<Tile>();
    }
  }

  public override int damageFormula() {
    return (int)(attributes.intelligence*(1+level*0.1));
  }
}

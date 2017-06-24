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
    targetAlly(false);
    targetEnemy(true);
  }

  public override List<Tile> getTargets() {
    return getTargetsInRange();
  }

  public List<Tile> getTargetsInAoe(Vector3 position) {
    Map map = GameManager.get.map;
    List<Tile> tiles = map.getCardinalTilesWithinRange(map.getTile(position), aoe);
    tiles.Add(map.getTile(position));
    return tiles;
  }

  public override int damageFormula() {
    return (int)(self.intelligence*(1+level*0.1));
  }
}

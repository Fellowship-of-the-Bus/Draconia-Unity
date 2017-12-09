using UnityEngine;
using System.Collections.Generic;

public class Sprint: ActiveSkill, AoeSkill {
  public int aoe {get; set;}
  public bool effectsTiles {get; set;}

  public Sprint() {
    //range = additional move range...
    range = 3;
    aoe = 0;
    useLos = false;
    name = "Sprint";
    effectsTiles = true;
    maxCooldown = 3;
    targetsTiles = true;

    dType = DamageType.magical;
    dEle = DamageElement.ice;
    targetAlly(true);
    targetEnemy(true);
  }

  public override List<Tile> getTargets(Tile posn) {
    //get tiles within move range,
    //filter by unoccupied
    GameManager gm = GameManager.get;
    Map map = gm.map;

    int rangeLeft = range + gm.moveRange;

    List<Tile> targets = map.getTilesWithinMovementRange(rangeLeft);
    return new List<Tile>(targets.Filter((x) => !x.occupied()));
  }

  public List<Tile> getTargetsInAoe(Vector3 position) {
    return getTargetsInAoe(position, aoe);
  }

  public override void tileEffects(Tile target) {
    //move to target
    GameManager.get.movePiece(self, target);
  }

  public override int damageFormula() {
    return 0;
  }
}

using UnityEngine;
using System.Collections.Generic;

public class CircleSlash: ActiveSkill, AoeSkill {
  public int aoe {get; set;}
  public bool effectsTiles {get; set;}

  public CircleSlash() {
    range = 0;
    useWepRange = false;
    aoe = 1;
    useLos = false;
    name = "CircleSlash";
    effectsTiles = false;
    maxCooldown = 2;
    targetsTiles = true;

    targetAlly(true);
    targetEnemy(true);
  }

  public override string tooltip { get { return "Range: Melee\n"
    + "Deal " + tooltipDamage + " damage to all characters in range"; }}

  public List<Tile> getTargetsInAoe(Vector3 position) {
    Map map = GameManager.get.map;
    List<Tile> targets = map.getTilesWithinRange(map.getTile(position), aoe);
    return targets;
  }

  public override int damageFormula() {
    return (int)(self.strength*(1+level*0.1));
  }
}

using UnityEngine;
using System.Collections.Generic;

public class CircleSlash: ActiveSkill, AoeSkill {
  public int aoe {get; set;}
  public bool effectsTiles {get; set;}

  public override string animation { get { return "CircleSlash"; }}

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

  protected override string tooltipDescription { get {
    return "Deal " + tooltipDamage + " damage to all characters in range";
  }}

  //implements AoeSkill....
  public List<Tile> getTargetsInAoe(Vector3 position) {
    Map map = GameManager.get.map;
    List<Tile> targets = map.getTilesWithinRange(map.getTile(position), aoe, usableWeapon[(int)Weapon.Kinds.Melee]);
    return targets;
  }

  public override int damageFormula() {
    return (int)(self.strength*(1+level*0.1));
  }
}

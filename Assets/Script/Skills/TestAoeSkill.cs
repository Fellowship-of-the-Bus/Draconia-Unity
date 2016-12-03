using UnityEngine;
using System.Collections.Generic;

public class TestAoeSkill: ActiveSkill, AoeSkill {
  public int aoe {get; set;}

  public TestAoeSkill() {
    range = 3;
    aoe = 2;
    useLos = false;
    name = "AOE";
  }
  public override void activate(List<Character> targets) {
    foreach (Character c in targets) {
      c.takeDamage(calculateDamage(self, c));
    }
  }

  public override List<GameObject> getTargets() {
    GameManager gm = GameManager.get;
    List<Tile> tiles = gm.getTilesWithinRange(self.curTile, range);
    List<GameObject> targets = new List<GameObject>();
    foreach (Tile t in tiles) {
      targets.Add(t.gameObject);
    }
    targets.Add(self.curTile.gameObject);
    return targets;
  }

  public List<GameObject> getTargetsInAoe(Vector3 position) {
    GameManager gm = GameManager.get;
    List<Tile> tiles = gm.getTilesWithinRange(gm.getTile(position), aoe);
    List<GameObject> targets = new List<GameObject>();
    foreach (Tile t in tiles) {
        if (t.occupant) targets.Add(t.occupant);
        else targets.Add(t.gameObject);
    }
    return targets;
  }


  public override int calculateDamage(Character source, Character target) {
    return (int)(source.attr.strength*(1+level*0.1) - target.attr.physicalDefense);
  }


}

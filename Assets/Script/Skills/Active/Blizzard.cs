using UnityEngine;
using System.Collections.Generic;

public class Blizzard: ActiveSkill, AoeSkill {
  public int aoe {get; set;}

  public Blizzard() {
    range = 3;
    aoe = 5;
    useLos = false;
    name = "Blizzard";
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
    return (int)((source.attr.intelligence*(1+level*0.1) - target.attr.magicDefense) * (100 - target.attr.iceResistance)/100f);
  }


}

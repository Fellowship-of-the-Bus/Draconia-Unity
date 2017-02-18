using UnityEngine;
using System.Collections.Generic;

public abstract class CircleAoE: ActiveSkill, AoeSkill {
  public int aoe {get; set;}
  public bool effectsTiles {get; set;}

  public CircleAoE() {
    targetsTiles = true;
  }

  public override List<GameObject> getTargets() {
    if (useWepRange) {
      range = self.weapon.range;
    }

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
    Tile cur = gm.getTile(position);
    List<Tile> tiles = gm.getTilesWithinRange(cur, aoe);
    List<GameObject> targets = new List<GameObject>();
    foreach (Tile t in tiles) {
        if (t.occupant) targets.Add(t.occupant);
        else targets.Add(t.gameObject);
    }
    if (cur.occupant) targets.Add(cur.occupant);
    else targets.Add(cur.gameObject);
    return targets;
  }
}

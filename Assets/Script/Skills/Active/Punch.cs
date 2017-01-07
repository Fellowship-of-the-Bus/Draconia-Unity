using UnityEngine;
using System.Collections.Generic;

public class Punch: ActiveSkill {
  public Punch() {
    range = 1;
    useLos = false;
    name = "Punch";
  }

  public override List<GameObject> getTargets() {
    List<Tile> tiles = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().getTilesWithinRange(self.curTile, 1);
    List<GameObject> targets = new List<GameObject>();
    foreach (Tile t in tiles) {
      if (t.occupied()) {
        targets.Add(t.occupant);
      }
    }
    return targets;
  }


  public override int calculateDamage(Character source, Character target) {
    return (int)(source.attr.strength*(1+level*0.1) - target.attr.physicalDefense);
  }
}

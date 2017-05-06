using UnityEngine;
using System.Collections.Generic;

public abstract class SelfTarget: ActiveSkill {
  public override List<Tile> getTargets() {
    List<Tile> targets = new List<Tile>();
    targets.Add(self.curTile);
    return targets;
  }
}

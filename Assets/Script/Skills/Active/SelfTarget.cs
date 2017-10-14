using UnityEngine;
using System.Collections.Generic;

public abstract class SelfTarget: ActiveSkill {
  public override List<Tile> getTargets(Tile posn) {
    List<Tile> targets = new List<Tile>();
    targets.Add(posn);
    return targets;
  }
}

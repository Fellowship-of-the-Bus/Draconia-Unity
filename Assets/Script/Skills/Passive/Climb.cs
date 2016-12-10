using UnityEngine;
using System.Collections.Generic;

public class Climb : PassiveSkill {
  public override List<GameObject> getTargets() {
    List<GameObject> targets = new List<GameObject>();
    targets.Add(self.gameObject);
    return targets;
  }

  public override void onActivate() {
    owner.moveTolerance += level;
  }
  public override void onDeactivate() {
    owner.moveTolerance -= level;
  }
}

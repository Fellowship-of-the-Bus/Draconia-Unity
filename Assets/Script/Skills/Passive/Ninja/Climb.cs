using UnityEngine;
using System.Collections.Generic;

public class Climb : PassiveSkill {
  public override List<GameObject> getTargets() {
    List<GameObject> targets = new List<GameObject>();
    targets.Add(self.gameObject);
    return targets;
  }

  protected override void onActivate() {
    owner.attrChange.moveTolerance += level;
  }
  protected override void onDeactivate() {
    owner.attrChange.moveTolerance -= level;
  }
}

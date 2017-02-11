using UnityEngine;
using System.Collections.Generic;

public abstract class SelfTarget: ActiveSkill {
  public override List<GameObject> getTargets() {
    List<GameObject> targets = new List<GameObject>();
    targets.Add(self.gameObject);
    return targets;
  }
}

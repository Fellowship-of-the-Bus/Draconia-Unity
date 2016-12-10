using UnityEngine;
using System.Collections.Generic;

public class StrengthBonus : PassiveSkill {
  public override List<GameObject> getTargets() {
    List<GameObject> targets = new List<GameObject>();
    targets.Add(self.gameObject);
    return targets;
  }

  public override void onActivate() {
    owner.attr.strength += level*2;
  }
  public override void onDeactivate() {
    owner.attr.strength -= level*2;
  }
}

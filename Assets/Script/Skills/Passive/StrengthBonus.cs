using UnityEngine;
using System.Collections.Generic;

public class StrengthBonus : PassiveSkill {
  public override List<GameObject> getTargets() {
    List<GameObject> targets = new List<GameObject>();
    targets.Add(self.gameObject);
    return targets;
  }

  protected override void onActivate() {
    owner.attr.strength += level*2;
  }
  protected override void onDeactivate() {
    owner.attr.strength -= level*2;
  }
}

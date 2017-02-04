using UnityEngine;
using System.Collections.Generic;

public class HealingBonus : PassiveSkill {
  public override List<GameObject> getTargets() {
    List<GameObject> targets = new List<GameObject>();
    targets.Add(self.gameObject);
    return targets;
  }

  protected override void onActivate() {
    owner.attrChange.healingMultiplier += level;
  }
  protected override void onDeactivate() {
    owner.attrChange.healingMultiplier -= level;
  }
}

using UnityEngine;
using System.Collections.Generic;

public class HealingBonus : PassiveSkill {
  public override List<GameObject> getTargets() {
    List<GameObject> targets = new List<GameObject>();
    targets.Add(self.gameObject);
    return targets;
  }

  public override void onActivate() {
    owner.attrChange.healingMultiplier += level;
  }
  public override void onDeactivate() {
    owner.attrChange.healingMultiplier -= level;
  }
}

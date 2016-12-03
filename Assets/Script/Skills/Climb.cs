using UnityEngine;
using System.Collections.Generic;

public class Climb : PassiveSkill {
  public override void activate(Character target) {
    ClimbEffect bonus = new ClimbEffect();
    bonus.level = level;
    target.applyEffect(bonus);
  }

  public override List<GameObject> getTargets() {
    List<GameObject> targets = new List<GameObject>();
    targets.Add(self.gameObject);
    return targets;
  }

}

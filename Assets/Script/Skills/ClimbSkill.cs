using UnityEngine;
using System.Collections.Generic;

public class ClimbSkill : PassiveSkill {
  public override void activate(List<Character> targets) {
    ClimbEffect bonus = new ClimbEffect();
    bonus.level = level;
    foreach (Character target in targets) {
      target.applyEffect(bonus);
    }
  }

  public override List<GameObject> getTargets() {
    List<GameObject> targets = new List<GameObject>();
    targets.Add(self.gameObject);
    return targets;
  }

}

using UnityEngine;
using System.Collections.Generic;

public class BerserkSkill : PassiveSkill {
  public override void activate(List<Character> targets) {
    BerserkEffect bonus = new BerserkEffect();
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

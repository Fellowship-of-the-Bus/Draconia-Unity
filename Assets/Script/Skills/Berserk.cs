using UnityEngine;
using System.Collections.Generic;

public class Berserk : PassiveSkill {
  public override void activate(Character target) {
    BerserkEffect bonus = new BerserkEffect();
    bonus.level = level;
    target.applyEffect(bonus);
  }

  public override List<GameObject> getTargets() {
    List<GameObject> targets = new List<GameObject>();
    targets.Add(self.gameObject);
    return targets;
  }

}

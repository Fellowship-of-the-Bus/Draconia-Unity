using UnityEngine;
using System.Collections.Generic;

public class StrengthBonus : PassiveSkill {
  public override void activate(Character target) {
    StrengthBonusEffect bonus = new StrengthBonusEffect();
    bonus.level = level;
    target.applyEffect(bonus);
  }

  public override List<GameObject> getTargets() {
    List<GameObject> targets = new List<GameObject>();
    targets.Add(self.gameObject);
    return targets;
  }

}

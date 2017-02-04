using UnityEngine;
using System.Collections.Generic;

public class HealingBonus : PassiveSkill {
  protected override void onActivate() {
    owner.attrChange.healingMultiplier += level;
  }
  protected override void onDeactivate() {
    owner.attrChange.healingMultiplier -= level;
  }
}

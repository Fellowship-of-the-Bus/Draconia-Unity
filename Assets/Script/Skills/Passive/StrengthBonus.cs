using UnityEngine;
using System.Collections.Generic;

public class StrengthBonus : PassiveSkill {
  protected override void onActivate() {
    owner.attr.strength += level*2;
  }
  protected override void onDeactivate() {
    owner.attr.strength -= level*2;
  }
}

using UnityEngine;
using System.Collections.Generic;

public class Climb : PassiveSkill {
  protected override void onActivate() {
    owner.attrChange.moveTolerance += level;
  }
  protected override void onDeactivate() {
    owner.attrChange.moveTolerance -= level;
  }
}

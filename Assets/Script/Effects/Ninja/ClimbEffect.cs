using UnityEngine;
using System.Collections.Generic;

public class ClimbEffect : DurationEffect {
  public ClimbEffect() {
    effectValue = 1000;
  }
  protected override void onActivate() {
    owner.attrChange.moveTolerance += (float)effectValue;
  }
  protected override void onDeactivateEffects() {
    owner.attrChange.moveTolerance -= (float)effectValue;
  }
}

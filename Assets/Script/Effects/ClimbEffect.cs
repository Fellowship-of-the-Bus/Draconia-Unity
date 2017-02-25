using UnityEngine;
using System.Collections.Generic;

public class ClimbEffect : DurationEffect {
  protected override void onActivate() {
    owner.attrChange.moveTolerance += 1000f;
  }
  protected override void onDeactivateEffects() {
    owner.attrChange.moveTolerance -= 1000f;
  }
}

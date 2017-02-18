using UnityEngine;

public class LevitateEffect : DurationEffect {
  protected override void onActivate() {
    owner.levitating = true;
  }
  protected override void onDeactivateEffects() {
    owner.levitating = false;
  }
}

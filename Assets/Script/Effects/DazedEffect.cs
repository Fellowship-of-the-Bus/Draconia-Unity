using UnityEngine;

public class DazedEffect : DurationEffect {
  public override void onActivate() {
    owner.attr.speed -= level;
  }
  public override void onDeactivateEffects() {
    owner.attr.speed += level;
  }
}

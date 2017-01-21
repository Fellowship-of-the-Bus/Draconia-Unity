using UnityEngine;

public class SlowEffect : DurationEffect {
  public override void onActivate() {
    owner.attr.speed -= level;
  }
  public override void onDeactivateEffects() {
    owner.attr.speed += level;
  }
}

using UnityEngine;

public class SlowEffect : DurationEffect {
  protected override void onActivate() {
    owner.attrChange.speed -= effectValue;
  }
  protected override void onDeactivateEffects() {
    owner.attrChange.speed += effectValue;
  }
}

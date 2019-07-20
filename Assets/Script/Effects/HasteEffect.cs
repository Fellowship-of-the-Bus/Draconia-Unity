using UnityEngine;

public class HasteEffect : DurationEffect {
  protected override void onActivate() {
    owner.attrChange.speed += effectValue;
  }
  protected override void onDeactivateEffects() {
    owner.attrChange.speed -= effectValue;
  }
}

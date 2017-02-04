using UnityEngine;

public class HasteEffect : DurationEffect {
  protected override void onActivate() {
    owner.attrChange.speed += level;
  }
  protected override void onDeactivateEffects() {
    owner.attrChange.speed -= level;
  }
}

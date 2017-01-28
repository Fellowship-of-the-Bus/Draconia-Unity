using UnityEngine;

public class HasteEffect : DurationEffect {
  public override void onActivate() {
    owner.attrChange.speed += level;
  }
  public override void onDeactivateEffects() {
    owner.attrChange.speed -= level;
  }
}

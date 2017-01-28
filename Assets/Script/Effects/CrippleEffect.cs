using UnityEngine;

public class CrippleEffect : DurationEffect {
  protected override void onActivate() {
    owner.attrChange.moveRange -= level;
  }
  protected override void onDeactivateEffects() {
    owner.attrChange.moveRange += level;
  }
}

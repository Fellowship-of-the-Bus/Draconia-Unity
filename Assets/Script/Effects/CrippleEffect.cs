using UnityEngine;

public class CrippleEffect : DurationEffect {
  public override void onActivate() {
    owner.attrChange.moveRange -= level;
  }
  public override void onDeactivateEffects() {
    owner.attrChange.moveRange += level;
  }
}

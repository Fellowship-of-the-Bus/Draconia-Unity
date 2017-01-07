using UnityEngine;

public class CrippleEffect : DurationEffect {
  public override void onActivate() {
    owner.moveRange -= level;
  }
  public override void onDeactivateEffects() {
    owner.moveRange += level;
  }
}

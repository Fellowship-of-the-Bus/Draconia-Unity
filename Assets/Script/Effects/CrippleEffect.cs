using UnityEngine;

public class CrippleEffect : DurationEffect {
  public CrippleEffect() {
    name = "Crippled";
  }

  protected override void onActivate() {
    owner.attrChange.moveRange -= level;
  }
  protected override void onDeactivateEffects() {
    owner.attrChange.moveRange += level;
  }
}

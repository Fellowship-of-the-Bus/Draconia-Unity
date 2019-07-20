using UnityEngine;

public class CrippleEffect : DurationEffect {
  public CrippleEffect() {
    name = "Crippled";
  }

  protected override string tooltipDescription { get {
  	return "Movement range reduced by 2";
  }}

  protected override void onActivate() {
    owner.attrChange.moveRange -= effectValue;
  }
  protected override void onDeactivateEffects() {
    owner.attrChange.moveRange += effectValue;
  }
}

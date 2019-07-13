using UnityEngine;

public class EmpowerEffect : DurationEffect {
  public EmpowerEffect() {
    stackable = true;
  }
  protected override void onActivate() {
    owner.attrChange.strength += effectValue;
  }
  protected override void onDeactivateEffects() {
    owner.attrChange.strength -= effectValue;
  }
}

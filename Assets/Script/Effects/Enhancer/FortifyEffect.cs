using UnityEngine;

public class FortifyEffect : DurationEffect {
  public FortifyEffect() {
    stackable = true;
  }
  protected override void onActivate() {
    owner.attrChange.physicalDefense += effectValue;
  }
  protected override void onDeactivateEffects() {
    owner.attrChange.physicalDefense -= effectValue;
  }
}

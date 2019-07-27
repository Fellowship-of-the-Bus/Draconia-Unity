using UnityEngine;

public class WardEffect : DurationEffect {
  public WardEffect() {
    stackable = true;
  }
  protected override void onActivate() {
    owner.attrChange.magicDefense += effectValue;
  }
  protected override void onDeactivateEffects() {
    owner.attrChange.magicDefense -= effectValue;
  }
}

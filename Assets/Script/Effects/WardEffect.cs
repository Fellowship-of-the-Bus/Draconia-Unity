using UnityEngine;

public class WardEffect : DurationEffect {
  public WardEffect() {
    stackable = true;
  }
  protected override void onActivate() {
    owner.attrChange.magicDefense += level;
  }
  protected override void onDeactivateEffects() {
    owner.attrChange.magicDefense -= level;
  }
}

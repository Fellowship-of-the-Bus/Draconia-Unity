using UnityEngine;

public class EnlightenEffect : DurationEffect {
  public EnlightenEffect() {
    stackable = true;
  }
  protected override void onActivate() {
    owner.attrChange.intelligence += level;
  }
  protected override void onDeactivateEffects() {
    owner.attrChange.intelligence -= level;
  }
}

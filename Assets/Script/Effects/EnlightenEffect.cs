using UnityEngine;

public class EnlightenEffect : DurationEffect {
  public EnlightenEffect() {
    stackable = true;
  }
  protected override void onActivate() {
    owner.attrChange.intelligence += effectValue;
  }
  protected override void onDeactivateEffects() {
    owner.attrChange.intelligence -= effectValue;
  }
}

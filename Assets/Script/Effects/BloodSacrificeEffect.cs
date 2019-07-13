using UnityEngine;

public class BloodSacrificeEffect : DurationEffect {
  private Attributes attr = new Attributes();
  public BloodSacrificeEffect() {
    stackable = true;
  }
  public void setValue(int val) {
    effectValue = val;
    attr.intelligence = effectValue;
    attr.strength = effectValue;
  }
  protected override void onActivate() {
    owner.attrChange += attr;
  }
  protected override void onDeactivateEffects() {
    owner.attrChange -= attr;
  }
}

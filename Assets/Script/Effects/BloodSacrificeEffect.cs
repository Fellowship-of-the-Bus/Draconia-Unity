using UnityEngine;

public class BloodSacrificeEffect : DurationEffect {
  private Attributes attr = new Attributes();
  public BloodSacrificeEffect() {
    stackable = true;
  }
  public void setLevel(int l) {
    level = l;
    attr.intelligence = level;
    attr.strength = level;
  }
  protected override void onActivate() {
    owner.attrChange += attr;
  }
  protected override void onDeactivateEffects() {
    owner.attrChange -= attr;
  }
}

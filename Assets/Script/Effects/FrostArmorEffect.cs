using UnityEngine;

public class FrostArmorEffect : DurationEffect {
  public override void onActivate() {
    owner.attr.physicalDefense += level;
    owner.attr.iceResistance += level;
  }
  public override void onDeactivateEffects() {
    owner.attr.physicalDefense -= level;
    owner.attr.iceResistance -= level;
  }
}

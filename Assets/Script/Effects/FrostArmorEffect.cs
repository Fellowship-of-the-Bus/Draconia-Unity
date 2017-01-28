using UnityEngine;

public class FrostArmorEffect : DurationEffect {
  protected override void onActivate() {
    owner.attr.physicalDefense += level;
    owner.attr.iceResistance += level;
  }
  protected override void onDeactivateEffects() {
    owner.attr.physicalDefense -= level;
    owner.attr.iceResistance -= level;
  }
}

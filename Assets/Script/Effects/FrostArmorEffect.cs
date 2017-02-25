using UnityEngine;

public class FrostArmorEffect : DurationEffect {
  protected override void onActivate() {
    owner.attrChange.physicalDefense += level;
    owner.attrChange.iceResistance += level;
  }
  protected override void onDeactivateEffects() {
    owner.attrChange.physicalDefense -= level;
    owner.attrChange.iceResistance -= level;
  }
}
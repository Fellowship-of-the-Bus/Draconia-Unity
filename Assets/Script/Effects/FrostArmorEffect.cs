using UnityEngine;

public class FrostArmorEffect : DurationEffect {
  private const int resistanceIncrease = 30;
  public FrostArmorEffect() {
    name = "Frost Armor";
  }

  protected override void onActivate() {
    owner.attrChange.physicalDefense += effectValue;
    owner.attrChange.iceResistance += resistanceIncrease;
  }
  protected override void onDeactivateEffects() {
    owner.attrChange.physicalDefense -= effectValue;
    owner.attrChange.iceResistance -= resistanceIncrease;
  }
}

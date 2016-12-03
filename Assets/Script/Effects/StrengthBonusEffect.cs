using UnityEngine;

public class StrengthBonusEffect : Effect {
  public override void onRemove() {

  }
  public override void onActivate() {
    owner.attr.strength += level*2;
  }
  public override void onDeactivate() {
    owner.attr.strength -= level*2;
  }
}

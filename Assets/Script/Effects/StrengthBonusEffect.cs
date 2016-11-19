using UnityEngine;

public class StrengthBonusEffect : Effect {
  public override void onRemove() {

  }
  public override void onActivate() {
    owner.attr.strength += level*5;
  }
  public override void onDeactivate() {
    owner.attr.strength -= level*5;
  }
  public override void activate() {

  }

  public override void onEvent(MonoBehaviour sender, EventHook hook) {

  }
}

using UnityEngine;

public class DazedEffect : Effect {
  public override void onActivate() {
    owner.attr.speed -= level;
  }
  public override void onDeactivate() {
    owner.attr.speed += level;
  }
}

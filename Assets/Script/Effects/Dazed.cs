using UnityEngine;

public class Dazed : Effect {
  public override void onActivate() {
    owner.attr.speed -= level;
  }
  public override void onDeactivate() {
    owner.attr.speed += level;
  }
}

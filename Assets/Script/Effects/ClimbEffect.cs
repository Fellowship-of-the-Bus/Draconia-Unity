using UnityEngine;

public class ClimbEffect : Effect {
  public override void onRemove() {

  }
  public override void onActivate() {
    owner.heightTolerance += level;
  }
  public override void onDeactivate() {
    owner.heightTolerance -= level;
  }
  public override void onEvent(Event e) {

  }
}

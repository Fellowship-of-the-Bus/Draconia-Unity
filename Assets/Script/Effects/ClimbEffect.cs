using UnityEngine;

public class ClimbEffect : Effect {
  public override void onRemove() {

  }
  public override void onActivate() {
    owner.moveTolerance += level;
  }
  public override void onDeactivate() {
    owner.moveTolerance -= level;
  }
}

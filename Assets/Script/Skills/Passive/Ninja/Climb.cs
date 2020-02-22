using UnityEngine;
using System.Collections.Generic;

public class Climb : PassiveSkill {
  public Climb() {
    name = "Climb";
  }

  public override string tooltipDescription { get {
    return "Increases maximum height difference during movement by 1 per level.";
  }}

  protected override void onActivate() {
    owner.attrChange.moveTolerance += level;
  }
  protected override void onDeactivate() {
    owner.attrChange.moveTolerance -= level;
  }
}

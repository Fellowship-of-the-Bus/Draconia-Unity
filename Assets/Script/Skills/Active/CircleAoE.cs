using UnityEngine;
using System.Collections.Generic;

public abstract class CircleAoE: ActiveSkill, AoeSkill {
  public int aoe {get; set;}
  public bool effectsTiles {get; set;}

  public string tooltipAoE { get { return "<b>Area of Effect:</b> " + aoe.ToString() + "\n"; }}
  public override string tooltipHeader { get { return base.tooltipHeader + tooltipAoE; }}

  public CircleAoE() {
    targetsTiles = true;
  }

  public virtual List<Tile> getTargetsInAoe(Vector3 position) {
    return getTargetsInAoe(position, aoe);
  }
}

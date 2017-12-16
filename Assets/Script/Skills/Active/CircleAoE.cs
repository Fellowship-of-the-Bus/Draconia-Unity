using UnityEngine;
using System.Collections.Generic;

public abstract class CircleAoE: ActiveSkill, AoeSkill {
  public int aoe {get; set;}
  public bool effectsTiles {get; set;}

  public string tooltipAoE { get { return "Area of Effect: " + aoe.ToString() + "\n"; }}
  public string tooltipHeader { get { return tooltipRange + tooltipAoE; }}

  public CircleAoE() {
    targetsTiles = true;
  }

  public virtual List<Tile> getTargetsInAoe(Vector3 position) {
    return getTargetsInAoe(position, aoe);
  }
}

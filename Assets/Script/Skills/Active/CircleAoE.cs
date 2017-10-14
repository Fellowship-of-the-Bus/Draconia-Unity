using UnityEngine;
using System.Collections.Generic;

public abstract class CircleAoE: ActiveSkill, AoeSkill {
  public int aoe {get; set;}
  public bool effectsTiles {get; set;}

  public CircleAoE() {
    targetsTiles = true;
  }

  public virtual List<Tile> getTargetsInAoe(Vector3 position) {
    return getTargetsInAoe(position, aoe);
  }
}

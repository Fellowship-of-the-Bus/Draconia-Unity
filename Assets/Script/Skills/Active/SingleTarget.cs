using UnityEngine;
using System.Collections.Generic;

public abstract class SingleTarget: ActiveSkill {
  public override List<Tile> getTargets(Tile posn) {
    GameManager gm = GameManager.get;
    List<Tile> tiles = gm.map.getTilesWithinRange(posn, range);
    List<Tile> targets = new List<Tile>();
    float height = self.gameObject.GetComponent<MeshFilter>().mesh.bounds.extents.y;
    Vector3 source = new Vector3(self.transform.position.x, self.transform.position.y + 2*height/3, self.transform.position.z);
    foreach (Tile t in tiles) {
      if (t.occupied()) {
        if (!useLos) {
          if (canTarget(t)) targets.Add(t);
        } else {
          GameObject o = t.occupant.gameObject;
          float heightOther = o.GetComponent<MeshFilter>().mesh.bounds.extents.y;
          Vector3 target = new Vector3(t.occupant.transform.position.x, t.occupant.transform.position.y + 2*heightOther/3, t.occupant.transform.position.z);
          if (gm.checkLine(source, target, 2*heightOther/3)) {
            if (canTarget(t)) targets.Add(t);
          }
        }
      }
    }
    if (canTargetSelf) targets.Add(posn);
    return targets;
  }
  public List<Tile> getTargetsInAoe(Tile position, bool heightAdvantage = false) {
    return getTargetsInAoe(position, 0, heightAdvantage);
  }
}

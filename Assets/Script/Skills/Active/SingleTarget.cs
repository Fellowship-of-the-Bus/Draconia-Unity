using UnityEngine;
using System.Collections.Generic;

public abstract class SingleTarget: ActiveSkill {
  public override List<GameObject> getTargets() {
    GameManager gm = GameManager.get;
    List<Tile> tiles = gm.map.getTilesWithinRange(self.curTile, Range);
    List<GameObject> targets = new List<GameObject>();
    float height = self.gameObject.GetComponent<MeshFilter>().mesh.bounds.extents.y;
    Vector3 source = new Vector3(self.curTile.transform.position.x, self.curTile.transform.position.y + 2*height/3, self.curTile.transform.position.z);
    foreach (Tile t in tiles) {
      if (t.occupied()) {
        GameObject o = t.occupant;
        if (!useLos) {
          targets.Add(o);
        } else {
          float heightOther = o.GetComponent<MeshFilter>().mesh.bounds.extents.y;
          Vector3 target = new Vector3(t.transform.position.x, t.transform.position.y + 2*heightOther/3, t.transform.position.z);
          RaycastHit hitInfo;
          if (gm.checkLine(source, target, out hitInfo)) {
            targets.Add(o);
          }
        }
      }
    }
    if (canTargetSelf) targets.Add(self.gameObject);
    return targets;
  }
}

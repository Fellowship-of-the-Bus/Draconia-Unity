using UnityEngine;
using System.Collections.Generic;

public abstract class RangedTargeting: ActiveSkill {
  public override List<GameObject> getTargets() {
    range = self.attr.weaponRange;

    GameManager gm = GameManager.get;
    List<Tile> tiles = gm.getTilesWithinRange(self.curTile, range);
    List<GameObject> targets = new List<GameObject>();
    Vector3 source = new Vector3(gm.SelectedPiece.transform.position.x, gm.SelectedPiece.transform.position.y + 0.25f, gm.SelectedPiece.transform.position.z);
    foreach (Tile t in tiles) {
      if (t.occupied()) {
        GameObject o = t.occupant;
        Vector3 target = new Vector3(o.transform.position.x, o.transform.position.y + 0.25f, o.transform.position.z);
        RaycastHit hitInfo;
        if (gm.checkLine(source, target, out hitInfo)) {
          targets.Add(o);
        }
      }
    }
    return targets;
  }
}

using UnityEngine;
using System.Collections.Generic;

public abstract class SingleTarget: ActiveSkill {
  public override List<Tile> getTargets(Tile posn) {
    GameManager gm = GameManager.get;
    List<Tile> tiles = gm.map.getTilesWithinRange(posn, range, usableWeapon[(int)Weapon.Kinds.Melee]);
    List<Tile> targets = new List<Tile>();
    Vector3 source = gm.getTargetingPostion(self.gameObject);
    foreach (Tile t in tiles) {
      if (t.occupied()) {
        if (!useLos) {
          if (canTarget(t)) targets.Add(t);
        } else {
          GameObject targetObject = t.occupant.gameObject;
          Vector3 target = gm.getTargetingPostion(targetObject);
          if (gm.checkLine(source, target, targetObject.GetComponent<Collider>())) {
            if (canTarget(t)) targets.Add(t);
          }
        }
      }
    }
    if (canTargetSelf) targets.Add(posn);
    return targets;
  }
  public List<Tile> getTargetsInAoe(Tile position) {
    return getTargetsInAoe(position, 0);
  }
}

using UnityEngine;
using System.Collections.Generic;

public class RangedSkill: ActiveSkill {
  public new int id = 1002;

  public RangedSkill() {
    range = 3;
    useLos = false;
  }
  public override void activate(List<Character> targets) {
    foreach (Character c in targets) {
      c.takeDamage(calculateDamage(self, c));
    }
  }
  public override List<GameObject> getTargets() {
    GameManager gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    List<Tile> tiles = gm.getTilesWithinRange(self.curTile, range);
    List<GameObject> targets = new List<GameObject>();
    Vector3 source = new Vector3(gm.SelectedPiece.transform.position.x, gm.SelectedPiece.transform.position.y + 0.25f, gm.SelectedPiece.transform.position.z);
    foreach (Tile t in tiles) {
      if (t.occupied()) {
        GameObject o = t.occupant;
        Vector3 target = new Vector3(o.transform.position.x, o.transform.position.y + 0.25f, o.transform.position.z);
        Vector3 toTarget = target - source;

        Ray ray = new Ray(source, toTarget.normalized);
        RaycastHit hitInfo;
        Physics.Raycast(ray, out hitInfo);
        if (hitInfo.collider.gameObject == o) {
           targets.Add(o);
        }
      }
    }
    return targets;
  }


  public override int calculateDamage(Character source, Character target) {
    return (int)(source.attr.strength*(1+level*0.1) - target.attr.physicalDefense);
  }


}

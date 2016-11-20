using UnityEngine;
using System.Collections.Generic;

public class TestAoeSkill: ActiveSkill, AoeSkill {
  public int aoe {get; set;}

  public TestAoeSkill() {
    range = 3;
    aoe = 2;
    useLos = false;
    name = "AOE";
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
    Vector3 source = new Vector3(gm.SelectedPiece.transform.position.x, gm.SelectedPiece.transform.position.y + 1.6f, gm.SelectedPiece.transform.position.z);
    foreach (Tile t in tiles) {
      Vector3 target = new Vector3(t.transform.position.x, t.transform.position.y + 0.5f, t.transform.position.z);
      RaycastHit hitInfo;
      if (gm.checkLine(source, target, out hitInfo, 0.5f)) {
         targets.Add(t.gameObject);
      }
    }
    return targets;
  }

  public List<GameObject> getTargetsInAoe(Vector3 position) {
    GameManager gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    List<Tile> tiles = gm.getTilesWithinRange(gm.getTile(position), aoe);
    List<GameObject> targets = new List<GameObject>();
    foreach (Tile t in tiles) {
        if (t.occupant) targets.Add(t.occupant);
        else targets.Add(t.gameObject);
    }
    return targets;
  }


  public override int calculateDamage(Character source, Character target) {
    return (int)(source.attr.strength*(1+level*0.1) - target.attr.physicalDefense);
  }


}

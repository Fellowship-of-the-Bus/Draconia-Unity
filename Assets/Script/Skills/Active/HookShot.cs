using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class HookShot: ActiveSkill {

  public HookShot() {
    useLos = false;
    name = "Hook Shot";
  }

  float upThreshold = 0.5f;

  Tile pullTo(Character c) {
    Vector3 heading =  self.gameObject.transform.position - c.gameObject.transform.position;
    Vector3 direction = heading / heading.magnitude;
    direction.x = Mathf.Round(direction.x);
    direction.z = Mathf.Round(direction.z);

    Tile t = GameManager.get.getTile(c.gameObject.transform.position + direction);
    return t;
  }

  public override void additionalEffects(Character c) {
    range = self.attr.weaponRange;

    Tile t = pullTo(c);
    if (t != null && !t.occupied() && ((GameManager.get.getHeight(t) + upThreshold) > GameManager.get.getHeight(t))) {
      GameManager.get.updateTile(c,t);
      LinkedList<Tile> tile = new LinkedList<Tile>();
      tile.AddFirst(t);
      GameManager.get.moving = true;
      GameManager.get.waitToEndTurn(GameManager.get.StartCoroutine(GameManager.get.IterateMove(tile, c.gameObject, false)));
    }
  }
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


  public override int calculateDamage(Character source, Character target) {
    return (int)(source.attr.strength*(1+level*0.1) - target.attr.physicalDefense);
  }


}

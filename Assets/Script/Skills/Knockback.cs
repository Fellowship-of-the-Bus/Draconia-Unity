using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Knockback: ActiveSkill {

  public Knockback() {
    range = 1;
    useLos = false;
    name = "Knockback";
  }

  float upThreshold = 0.5f;

  Tile knockTo(Character c) {
      Vector3 heading = c.gameObject.transform.position - self.gameObject.transform.position;
      Vector3 direction = heading / heading.magnitude;
      Tile t = GameManager.get.getTile(c.gameObject.transform.position + direction);
      return t;
  }

  public override void activate(List<Character> targets) {
    foreach (Character c in targets) {
      Tile t = knockTo(c);
      if (t != null && !t.occupied() && ((GameManager.get.getHeight(t) + upThreshold) > GameManager.get.getHeight(t))) {
        GameManager.get.updateTile(c,t);
        LinkedList<Tile> tile = new LinkedList<Tile>();
        tile.AddFirst(t);
        GameManager.get.moving = true;
        GameManager.get.waitToEndTurn(GameManager.get.StartCoroutine(GameManager.get.IterateMove(tile, c.gameObject, false)));
      }
      c.takeDamage(calculateDamage(self, c));
    }
  }
  public override List<GameObject> getTargets() {
    List<Tile> tiles = GameManager.get.getTilesWithinRange(self.curTile, 1);
    List<GameObject> targets = new List<GameObject>();
    foreach (Tile t in tiles) {
      if (t.occupied()) {
        targets.Add(t.occupant);
      }
    }
    return targets;
  }


  public override int calculateDamage(Character source, Character target) {
    return (int)(source.attr.strength*(1+level*0.1) - target.attr.physicalDefense);
  }


}

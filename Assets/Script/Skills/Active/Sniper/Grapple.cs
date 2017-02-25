using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Grapple: CircleAoE {

  public Grapple() {
    targetsTiles = true;
    range = 1;
    aoe = 0;
    name = "Grapple";
    maxCooldown = 2;
    effectsTiles = true;
  }

  public override List<GameObject> getTargets() {
    List<GameObject> targets = base.getTargets();

    return new List<GameObject>(targets.Filter((tile) => tile.GetComponent<Tile>() != null && !tile.GetComponent<Tile>().occupied()));
  }

  public override void tileEffects(Tile t) {
    Tile curTile = GameManager.get.getTile(self.gameObject.transform.position);

    if (t != null && !t.occupied()) {
      GameManager.get.updateTile(self,t);
      LinkedList<Tile> path = new LinkedList<Tile>();
      path.AddFirst(t);
      GameManager.get.moving = true;
      GameManager.get.waitToEndTurn(GameManager.get.StartCoroutine(GameManager.get.IterateMove(path, self.gameObject)));
    }
  }
}

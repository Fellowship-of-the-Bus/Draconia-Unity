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
    targetAlly(true);
    targetEnemy(true);
  }

  public override List<Tile> getTargets(Tile posn) {
    List<Tile> targets = base.getTargets(posn);
    return new List<Tile>(targets.Filter(tile => !tile.occupied()));
  }

  public override void tileEffects(Tile t) {
    if (t != null && !t.occupied()) {
      GameManager.get.updateTile(self,t);
      LinkedList<Tile> path = new LinkedList<Tile>();
      path.AddFirst(t);
      GameManager.get.moving = true;
      GameManager.get.waitFor(GameManager.get.StartCoroutine(GameManager.get.IterateMove(path, self.gameObject, GameManager.get.getWaitingIndex())));
    }
  }
}

using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class TargetMover : SingleTarget {

  public enum Direction {
    towards,
    away
  };

  Direction direction = Direction.away;
  int distance = 1;

  protected void setDirection(Direction dir) {
    direction = dir;
  }

  protected void setDistance(int dist) {
    distance = dist;
  }

  float upThreshold = 0.5f;

  bool validTile(BattleCharacter c, Tile t) {
    GameManager game = GameManager.get;
    Map map = game.map;
    return t != null && ((map.getHeight(c.curTile) + upThreshold) > map.getHeight(t)) && !t.occupied();
  }

  LinkedList<Tile> movePath(BattleCharacter c) {
    LinkedList<Tile> path = new LinkedList<Tile>();
    Vector3 heading;
    if (this.direction == Direction.towards) {
      heading =  self.transform.position - c.transform.position;
    } else {
      heading =  c.transform.position - self.transform.position;
    }
    Vector3 direction = heading / heading.magnitude;
    direction.x = Mathf.Round(direction.x);
    direction.z = Mathf.Round(direction.z);
    Tile t = null;
    for(int i = 1; i <= distance; i++) {
      Tile t2 = GameManager.get.map.getTile(c.transform.position + (direction * i));
      if (validTile(c, t2)) {
        t = t2;
        path.AddLast(t);
      } else break; //Don't try to push through invalid tile to find a valid one
    }
    return path;
  }

  public override void additionalEffects(BattleCharacter c) {
    GameManager game = GameManager.get;
    LinkedList<Tile> path = movePath(c);
    Tile destination = path.Last.Value;
    if (validTile(c, destination) && c.isAlive()) {
      game.movePiece(c, path, false);
    }
  }

}

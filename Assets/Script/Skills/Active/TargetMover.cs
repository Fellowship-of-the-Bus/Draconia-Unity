using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class TargetMover : SingleTarget {

  public enum Direction {
    towards,
    away
  };

  Direction direction = Direction.away;

  protected void setDirection(Direction dir) {
    direction = dir;
  }

  float upThreshold = 0.5f;

  bool validTile(BattleCharacter c, Tile t) {
    GameManager game = GameManager.get;
    Map map = game.map;
    return t != null && ((map.getHeight(c.curTile) + upThreshold) > map.getHeight(t)) && !t.occupied();
  }

  Tile moveTo(BattleCharacter c) {
    Vector3 heading;
    if (this.direction == Direction.towards) {
      heading =  self.transform.position - c.transform.position;
    } else {
      heading =  c.transform.position - self.transform.position;
    }
    Vector3 direction = heading / heading.magnitude;
    direction.x = Mathf.Round(direction.x);
    direction.z = Mathf.Round(direction.z);

    Tile t = GameManager.get.map.getTile(c.transform.position + direction);
    Tile t2 = GameManager.get.map.getTile(c.transform.position + (direction * 2));
    if (validTile(c, t)) {
      if (validTile(c, t2)) {
        return t2;
      }
      return t;
    }
    return null;
  }

  public override void additionalEffects(BattleCharacter c) {
    GameManager game = GameManager.get;

    Tile t = moveTo(c);
    if (validTile(c, t) && c.isAlive()) {
      game.movePiece(c,t,false);
    }
  }

}
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;

public class BasicMoveAI : BaseMoveAI {
  public override Vector3 move() {
    GameManager game = GameManager.get;
    List<GameObject> characterObjects = game.players;
    Map map = game.map;

    int minDistance = Int32.MaxValue/2;
    foreach (GameObject o in characterObjects) {
      map.setPath(o.transform.position);
      if (map.path.Count < minDistance) {
        minDistance = map.getTile(o.transform.position).distance;
        break;
      }
    }
    Tile destination = owner.curTile;

    for (LinkedListNode<Tile> t = map.path.Last; t != map.path.First; t = t.Previous) {
      if (t.Value.distance <= owner.moveRange && !t.Value.occupied()) {
        destination = t.Value;
        break;
      }
    }
    return destination.gameObject.transform.position;
  }
}

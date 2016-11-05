using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;

public class BasicMoveAI : BaseMoveAI {
  public override Vector3 move() {
    GameManager game = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    GameObject[] characterObjects = GameObject.FindGameObjectsWithTag("PiecePlayer1");

    int minDistance = Int32.MaxValue/2;
    foreach (GameObject o in characterObjects) {
      game.setPath(o.transform.position);
      if (game.path.Count < minDistance) {
        minDistance = game.getTile(o.transform.position).distance;
        break;
      }
    }
    Tile destination = owner.curTile;

    //Debug.Log(game.path.Count);
    for (LinkedListNode<Tile> t = game.path.Last; t != game.path.First; t = t.Previous) {
      //Debug.Log("loop");
      if (t.Value.distance <= owner.moveRange && !t.Value.occupied()) {
        destination = t.Value;
        break;
      }
    }
    return destination.gameObject.transform.position;
  }
}

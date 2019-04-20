using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Linq;

public class SentryAI : BaseAI {
  Tile startTile = null;

  public override void init() {
    startTile = owner.curTile;
  }

  public override Vector3 move() {
    GameManager game = GameManager.get;
    Map map = game.map;

    Vector3 newPosition = owner.curTile.transform.position;
    Tile newTile = owner.curTile;
    // Determine movement when not on starting tile
    if (owner.curTile != startTile && startTile.distance != System.Int32.MaxValue) {
      LinkedList<Tile> path = map.getPath(startTile.transform.position);

      foreach (Tile t in path) {
        if (t.distance > owner.moveRange) {
          break;
        }
        newPosition = t.transform.position;
        newTile = t;
      }
    }
    best = evaluateSkillOptions(newTile);
    if (best != null) {
      GameManager.get.SelectedSkill = best.index;
      GameManager.get.selectTarget(best.targetTile.occupant.gameObject);
    }

    map.setPath(newPosition);
    return newPosition;
  }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Linq;

public class SentryAI : BaseAI {
  SkillData best;
  Tile startTile = null;

  public override void init() {
    startTile = owner.curTile;
  }

  public override void target() {
    if (best != null) {
      GameManager.get.SelectedSkill = best.index;

      List<Tile> target = new List<Tile>();
      target.Add(best.targetTile);
      owner.useSkill(best.skill, new List<Tile>(target));
    }
  }

  public override Vector3 move() {
    GameManager game = GameManager.get;
    Map map = game.map;
    List<GameObject> characterObjects = game.players;

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

    map.setPath(newPosition);
    return newPosition;
  }
}

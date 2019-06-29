using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Linq;

public class BuffAI : BaseAI {

  public override Vector3 move() {
    Heap<SkillData> db = new Heap<SkillData>();
    GameManager game = GameManager.get;
    Map map = game.map;
    Vector3 newPosition;

    List<Tile> possibilities = map.tilesInMoveRange(owner);
    possibilities.Add(owner.curTile);

    foreach (Tile tile in possibilities) {
      SkillData bestForTile = evaluateSkillOptions(tile);
      if (bestForTile != null) {
        db.add(bestForTile);
      }
    }
    best = db.getMax();

    // Determine movement when there are no valid attacks
    if (best == null) {
      newPosition = owner.curTile.transform.position;
    } else {
      newPosition = best.tile.transform.position;
      GameManager.get.SelectedSkill = best.index;
      GameManager.get.selectTarget(best.targetTile.occupant.gameObject, best.tile);
    }

    map.setPath(newPosition);
    return newPosition;
  }
}

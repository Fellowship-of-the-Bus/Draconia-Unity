using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Linq;

public class AggressiveAI : BaseAI {
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
      int closest = System.Int32.MaxValue;
      LinkedList<Tile> path = null;

      // Find closest enemy
      foreach(var kvp in game.characters) {
        Team team = kvp.Key;
        if (owner.isEnemyOf(team)) {
          List<BattleCharacter> characterObjects = kvp.Value;
          foreach (BattleCharacter bc in characterObjects) {
            if (bc.isEnemyOf(owner)) {
              if (bc.curTile.distance < closest) {
                closest = bc.curTile.distance;
                path = map.getPath(bc.curTile.transform.position);
              }
            }
          }
        }
      }

      // Find reachable tile closest to chosen enemy
      if (closest != System.Int32.MaxValue) {
        foreach (Tile t in path) {
          if (t.distance > owner.moveRange) {
            break;
          } else if (!t.occupied()) {
            newPosition = t.transform.position;
          }
        }
      }
    } else {
      newPosition = best.tile.transform.position;
      GameManager.get.SelectedSkill = best.index;
      //using temp skill
      if (best.index == BattleCharacter.numPermSkills) {
        owner.equippedSkills[BattleCharacter.numPermSkills] = best.skill;
      }
      GameManager.get.selectTarget(best.targetTile.occupant.gameObject, best.tile);
    }

    map.setPath(newPosition);
    return newPosition;
  }
}

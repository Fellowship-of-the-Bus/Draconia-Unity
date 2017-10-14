using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Linq;

public class BasicAI : BaseAI {
  SkillData best;

  public override void target() {
    if (best != null) {
      GameManager.get.SelectedSkill = best.index;

      List<Tile> target = new List<Tile>();
      target.Add(best.targetTile);
      owner.useSkill(best.skill, target);
    }
  }

  public override Vector3 move() {
    Heap<SkillData> db = new Heap<SkillData>();
    GameManager game = GameManager.get;
    Map map = game.map;
    List<GameObject> characterObjects = game.players;
    Vector3 newPosition;

    List<Tile> possibilities = map.tilesInMoveRange(owner);
    possibilities.Add(owner.curTile);

    foreach (Tile tile in possibilities) {
      int index = 0;
      foreach (ActiveSkill skill in owner.equippedSkills) {
        int cur = index++;

        // Skip unusable skills
        if (! skill.canUse()) continue;
        List<Tile> targets = skill.getTargets(tile);
        if (targets.Count == 0) continue;

        List<TargetSet> targetCharacters = getTargetSets(skill, targets);

        // Calculate net change in team health difference
        foreach (TargetSet tSet in targetCharacters) {
          List<BattleCharacter> c = tSet.affected;
          List<Effected> e = new List<Effected>();
          int damage = 0;

          foreach (BattleCharacter ch in c) {
            if (ch.team != owner.team) {
              damage += skill.calculateDamage(ch);
            } else {
              damage -= skill.calculateDamage(ch);
            }
            e.Add(ch);
          }

          if (damage > 0) {
            db.add(new SkillData(this, cur, damage, e, tile, tSet.tile));
          }
        }
      }
    }
    best = db.getMax();

    // Determine movement when there are no valid attacks
    if (best == null) {
      newPosition = owner.curTile.transform.position;
      int closest = System.Int32.MaxValue;
      LinkedList<Tile> path = null;

      // Find closest enemy
      foreach (GameObject c in characterObjects) {
        BattleCharacter bc = c.GetComponent<BattleCharacter>();
        if (bc.team != owner.team) {
          if (bc.curTile.distance < closest) {
            closest = bc.curTile.distance;
            path = map.getPath(bc.curTile.transform.position);
          }
        }
      }

      // Find reachable tile closest to chosen enemy
      if (closest != System.Int32.MaxValue) {
        foreach (Tile t in path) {
          if (t.distance > owner.moveRange) {
            break;
          }
          newPosition = t.transform.position;
        }
      }
    } else {
      newPosition = best.tile.transform.position;
    }

    map.setPath(newPosition);
    return newPosition;
  }
}

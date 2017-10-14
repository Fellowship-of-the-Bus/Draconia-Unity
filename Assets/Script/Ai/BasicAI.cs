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
        if (!skill.canUse()) continue;
        List<Tile> targets = skill.getTargets(tile);
        if (targets.Count == 0) continue;

        List<TargetSet> targetCharacters = getTargetSets(skill, targets, tile);

        // Calculate net change in team health difference
        foreach (TargetSet tSet in targetCharacters) {
          List<BattleCharacter> c = tSet.affected;
          List<Effected> effected = new List<Effected>();
          int netChange = 0;

          foreach (BattleCharacter ch in c) {
            if (skill is HealingSkill) {
              int val = Math.Min(skill.calculateHealing(ch), ch.maxHealth - ch.curHealth);
              if (ch.team != owner.team) {
                netChange -= val;
              } else {
                netChange += val;
              }
            } else {
              int val = skill.calculateDamage(ch);
              if (ch.team != owner.team) {
                netChange += val;
              } else {
                netChange -= val;
              }
            }
            
            effected.Add(ch);
          }

          if (netChange > 0) {
            db.add(new SkillData(this, cur, netChange, effected, tile, tSet.tile));
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

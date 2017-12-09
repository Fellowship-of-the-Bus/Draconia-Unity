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
    Heap<SkillData> db = new Heap<SkillData>();
    GameManager game = GameManager.get;
    Map map = game.map;
    List<GameObject> characterObjects = game.players;

    Vector3 newPosition = owner.curTile.transform.position;
    // Determine movement when not on starting tile
    if (owner.curTile != startTile && startTile.distance != System.Int32.MaxValue) {
      LinkedList<Tile> path = map.getPath(startTile.transform.position);

      foreach (Tile t in path) {
        if (t.distance > owner.moveRange) {
          break;
        }
        newPosition = t.transform.position;
      }
    }

    int index = 0;
    foreach (ActiveSkill skill in owner.equippedSkills) {
      int cur = index++;

      // Skip unusable skills
      if (! skill.canUse()) continue;
      List<Tile> targets = skill.getTargets();
      if (targets.Count == 0) continue;

      List<TargetSet> targetCharacters = getTargetSets(skill, targets, owner.curTile);

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
          db.add(new SkillData(this, cur, damage, e, owner.curTile, tSet.tile));
        }
      }
    }
    best = db.getMax();

    map.setPath(newPosition);
    return newPosition;
  }
}

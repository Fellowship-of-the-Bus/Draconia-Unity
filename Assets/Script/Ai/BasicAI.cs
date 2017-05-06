using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;

public class BasicAI : BaseAI {
  SkillData best;

  public override void target() {
    if (best != null) {
      GameManager.get.SelectedSkill = best.index;
      owner.attackWithSkill(best.skill, best.effected);
    }
  }

  public override Vector3 move() {
    Heap<SkillData> db = new Heap<SkillData>();

    GameManager game = GameManager.get;
    Map map = game.map;
    List<GameObject> characterObjects = game.players;

    List<Tile> possibilities = map.tilesInMoveRange(owner);
    possibilities.Add(owner.curTile);

    // Debug.Log("=======" + owner.name + "=======");
    foreach (Tile tile in possibilities) {
      owner.curTile = tile;
      int index = 0;
      // Debug.Log("Location: " + tile.transform.position);
      foreach (ActiveSkill skill in owner.equippedSkills) {
        int cur = index++;
        if (! skill.canUse()) continue;
        List<Tile> targets = skill.getTargets();
        // Debug.Log("Skill " + cur + ", " + skill.name + ", num targets: " + targets.Count);
        if (targets.Count == 0) continue;
        AoeSkill aoe = skill as AoeSkill;

        List<List<BattleCharacter>> targetCharacters = null;
        if (aoe != null) {
          foreach(Tile t in targets) {
            // Tile t = (Tile)obj;

            // targetCharacters.Add(aoe.getTargetsInAoe(t.gameObject.transform.position));
          }
          // c = new List<BattleCharacter>()
        } else {
          targetCharacters = new List<List<BattleCharacter>>();
          List<BattleCharacter> chars = new List<BattleCharacter>(targets.Select(x => x.occupant));
          chars = new List<BattleCharacter>(chars.Filter((character) => character != null && character.team != owner.team));
          targetCharacters.Add(chars);
        }

        foreach (List<BattleCharacter> c in targetCharacters) {
          List<Effected> e = new List<Effected>();
          int damage = 0;
          foreach (BattleCharacter ch in c) {
            damage = skill.calculateDamage(ch);
            // Debug.Log("character: " + ch.name + " damage: " + damage);
            e.Add(ch);
          }
          db.add(new SkillData(this, cur, damage, e, tile));
        }
      }
    }
    best = db.getMax();
    Vector3 newPosition = best == null ? owner.curTile.transform.position : best.tile.transform.position;
    map.setPath(newPosition);
    // int damage = best == null ? 0 : best.score;
    // Debug.Log("Location: " + newPosition + " damage: " + best.score);
    return newPosition;
  }
}

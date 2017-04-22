using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;

public class BasicAI : BaseAI {
  private class SkillData : IComparable<SkillData> {
    public int index;  // index in equippedSkills
    public int score;  // how good is this skill right now?
    public ActiveSkill skill;
    public List<Effected> effected;
    public Tile tile;  // location from which skill is used

    public SkillData(BasicAI ai, int index, int score, List<Effected> effected, Tile tile) {
      this.index = index;
      this.score = score;
      this.effected = effected;
      this.skill = ai.owner.equippedSkills[index];
      this.tile = tile;
    }

    public int CompareTo(SkillData other) {
      return score.CompareTo(other.score);
    }
  }

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
        List<GameObject> targets = skill.getTargets();
        // Debug.Log("Skill " + cur + ", " + skill.name + ", num targets: " + targets.Count);
        if (targets.Count == 0) continue;

        List<Character> c = new List<Character>(targets.Select(x => x.GetComponent<Character>()));
        c = new List<Character>(c.Filter((character) => character.team != owner.team));

        foreach (Character ch in c) {
          int damage = skill.calculateDamage(ch);
          // Debug.Log("character: " + ch.name + " damage: " + damage);
          List<Effected> e = new List<Effected>();
          e.Add(ch);
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

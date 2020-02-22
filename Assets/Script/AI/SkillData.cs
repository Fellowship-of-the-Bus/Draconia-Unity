using System.Collections.Generic;
using System;
using UnityEngine;

// information about a skill's usage on a target - for use by AI
public class SkillData : IComparable<SkillData> {
  public int index;  // index in equippedSkills
  public int score;  // how good is this skill right now?
  public ActiveSkill skill;
  public List<Effected> effected;
  public Tile tile;  // location from which skill is used
  public Tile targetTile;  // tile targeted by the skill

  public SkillData(BaseAI ai, int index, int score, List<Effected> effected,  ActiveSkill skill, Tile tile, Tile targetTile) {
    this.index = index;
    this.score = score;
    this.effected = effected;
    this.skill = skill;
    this.tile = tile;
    this.targetTile = targetTile;
  }

  public int CompareTo(SkillData other) {
    int diff = score.CompareTo(other.score);

    if (diff == 0) {
      return -tile.distance.CompareTo(other.tile.distance);
    } else {
      return diff;
    }
  }
}

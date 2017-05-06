using System.Collections.Generic;
using System;

// information about a skill's usage on a target - for use by AI
public class SkillData : IComparable<SkillData> {
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

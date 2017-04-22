using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SkillTree {
  Character self;

  public SkillTree(Character c) {
    self = c;
  }

  PassiveSkill makePassive<T>(int level = 1) where T : PassiveSkill, new() {
    T skill = new T();
    skill.level = level;
    skill.self = self;
    return skill;
  }

  //just return something for testing for now
  public List<PassiveSkill> getPassives() {
    List<PassiveSkill> passives = new List<PassiveSkill>();
    passives.Add(makePassive<StrengthBonus>());
    return passives;
  }
}



using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SkillTree {
  List<PassiveSkill> passives = new List<PassiveSkill>();
  List<ActiveSkill> actives = new List<ActiveSkill>();

  public void addSkill<T>(int level = 1) where T : Skill, new() {
    T skill = new T();
    skill.level = level;

    if (skill as PassiveSkill != null) {
      passives.Add(skill as PassiveSkill);
    } else if (skill as ActiveSkill != null) {
      actives.Add(skill as ActiveSkill);
    }
  }

  //just return something for testing for now
  public List<PassiveSkill> getPassives(BattleCharacter self) {
    foreach (PassiveSkill p in passives) {
      p.self = self;
    }

    return passives;
  }

  public List<ActiveSkill> getActives(BattleCharacter self) {
    foreach (ActiveSkill a in actives) {
      a.self = self;
    }

    return actives;
  }
  //called when character levels up
  public void gainLevels(int levelsToGain) {

  }

}



using UnityEngine;
using System;
using System.Reflection;
using System.Collections.Generic;

[System.Serializable]
public class SkillTree {
  Dictionary<Type, int> passives = new Dictionary<Type, int>();
  Dictionary<Type, int> actives = new Dictionary<Type, int>();

  List<Type> equippedSkills = new List<Type>();

  public int numSkillPoints = 0;
  public int level = 1;

  public SkillTree() {
    foreach(Type t in SkillList.get.skills) {
      addSkill(t, 0);
    }
  }

  private Type pskill = Type.GetType("PassiveSkill");
  private Type askill = Type.GetType("ActiveSkill");

  public void addSkill(Type t, int level = 1) {
    if (t.IsSubclassOf(pskill)) {
      passives.Add(t, level);
    } else {
      Debug.Assert(t.IsSubclassOf(askill));
      actives.Add(t, level);
    }
  }

  //just return something for testing for now
  public List<PassiveSkill> getPassives(BattleCharacter self) {
    List<PassiveSkill> l = new List<PassiveSkill>();
    foreach ( KeyValuePair<Type, int> kvp in passives) {
      if (kvp.Value == 0) continue;
      PassiveSkill p = (PassiveSkill)Activator.CreateInstance(kvp.Key);
      p.level = kvp.Value;
      p.self = self;
      l.Add(p);
    }

    return l;
  }

  public List<ActiveSkill> getActives(BattleCharacter self) {
    List<ActiveSkill> l = new List<ActiveSkill>();
    foreach (Type t in equippedSkills) {
      ActiveSkill a = (ActiveSkill)Activator.CreateInstance(t);
      a.level = actives[t];
      a.self = self;
      l.Add(a);
    }

    return l;
  }

  public int getSkillLevel(Type t) {
    if (passives.ContainsKey(t)) return passives[t];
    else if (actives.ContainsKey(t)) return actives[t];
    else {
      // Must be a new skill
      addSkill(t, 0);
      return 0;
    }
  }

  public void setSkillLevel(Type t, int lvl) {
    if (passives.ContainsKey(t)) passives[t] = lvl;
    else if (actives.ContainsKey(t)) actives[t] = lvl;
    else {
      // Must be a new skill
      addSkill(t, lvl);
    }
  }

  public void equipSkill(Type t) {
    equippedSkills.Add(t);
  }

  public void unequipSkill(Type t) {
    equippedSkills.Remove(t);
  }

  public bool isEquipped(Type t) {
    return equippedSkills.Contains(t);
  }

  public List<Type> getEquippedSkills() {
    return equippedSkills;
  }

  public bool isActive(Type t) {
    return actives.ContainsKey(t);
  }

  //called when character levels up
  public void gainLevels(int levelsToGain) {
    numSkillPoints += skillPointsAtLevel(level+levelsToGain) - skillPointsAtLevel(level);
    level += levelsToGain;
  }
  public static int skillPointsAtLevel(int level){
    int quotient = level/10;
    int remainder = level%10;
    return 5*quotient*(quotient+1) + remainder*(quotient+1) - 1;
  }

  // Get the total number of skill points allocated to a given specialization
  public int getSpecializationPoints(string spec) {
    int totalLevel = 0;
    Type[][] entry = SkillList.skillsByClass[spec];
    foreach(Type[] skillTier in entry) {
      foreach(Type skillType in skillTier) {
        int skillLevel = 0;
        if (passives.ContainsKey(skillType)) {
          skillLevel = passives[skillType];
        } else if (actives.ContainsKey(skillType)) {
          skillLevel = actives[skillType];
        }

        totalLevel += skillLevel;
      }
    }

    return totalLevel;
  }

  int[] tierRequirements = new int[] {0, 1, 4, 10, 20};
  // Get the tier of skills available for a given specialization
  public int getSpecializationTier(string spec) {
    int specPoints = getSpecializationPoints(spec);
    int tier = 0;
    while (tier < tierRequirements.Length - 1 && specPoints >= tierRequirements[tier + 1]) {
      tier++;
    }

    return tier;
  }
}

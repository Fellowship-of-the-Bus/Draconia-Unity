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
    else return actives[t];
  }

  public void setSkillLevel(Type t, int lvl) {
    if (passives.ContainsKey(t)) passives[t] = lvl;
    else actives[t] = lvl;
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

  }

}



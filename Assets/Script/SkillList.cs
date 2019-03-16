using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class SkillList {
  public readonly static SkillList get = new SkillList();

  public List<Type> skills = new List<Type>();
  public Dictionary<Type, Sprite> skillImages = new Dictionary<Type, Sprite>();

  private SkillList() {
    Type skill = Type.GetType("Skill");
    foreach (Type t in Assembly.GetExecutingAssembly().GetTypes()) {
      if (skill.IsAssignableFrom(t) && !t.IsAbstract) {
        skills.Add(t);
      }
    }
    skills.Sort((a,b) => {
      return String.Compare(a.displayName(), b.displayName());
    });
  }

  bool dictCreated = false;
  public void createDict() {
    if (dictCreated) return;
    dictCreated = true;
    Type skill = Type.GetType("Skill");
    foreach (Type t in Assembly.GetExecutingAssembly().GetTypes()) {
      if (skill.IsAssignableFrom(t) && !t.IsAbstract) {
        skillImages.Add(t, Resources.Load<Sprite>("Skill Images/" + t.ToString()));
      }
    }
  }
}

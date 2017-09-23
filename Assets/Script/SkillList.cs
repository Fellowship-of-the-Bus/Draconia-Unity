using System;
using System.Reflection;
using System.Collections.Generic;

class SkillList {
  public static SkillList s = new SkillList();
  public static SkillList get { get; set; }

  public List<Type> skills = new List<Type>();

  private SkillList() {
    get = this;
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
}
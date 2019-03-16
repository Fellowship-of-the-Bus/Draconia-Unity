using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SkillSelectController: MonoBehaviour {
  private const int NUM_EQUIPPED_SKILLS = 4;

  public GameObject skillInfo;
  public InvCharSelect charSelect;
  public Transform skillView;
  public Transform equippedSkillView;

  List<SkillInfo> equippedSkills  = new List<SkillInfo>();
  List<SkillInfo> skills = new List<SkillInfo>();

  Character curChar;

  void Awake() {
    get = this;

    foreach(Type t in SkillList.get.skills) {
      GameObject o = Instantiate(skillInfo, skillView);
      SkillInfo s = o.GetComponent<SkillInfo>();
      s.init(t, false);
      skills.Add(s);
    }

  }

  public void setChar(Character c) {
    curChar = c;
    foreach(SkillInfo s in equippedSkills) {
      GameObject.Destroy(s.gameObject);
    }
    equippedSkills.Clear();
    foreach (SkillInfo s in skills) {
      s.update(c.skills);
    }
    foreach(Type t in c.skills.getEquippedSkills()) {
      equip(t);
    }
  }

  public SkillInfo equip(Type t) {
    if (equippedSkills.Count < NUM_EQUIPPED_SKILLS) {
      GameObject o = Instantiate(skillInfo, equippedSkillView);
      SkillInfo s = o.GetComponent<SkillInfo>();
      s.init(t, true);
      s.update(curChar.skills);
      equippedSkills.Add(s);
      return s;
    }
    return null;
  }

  public void unequip(SkillInfo s) {
    equippedSkills.Remove(s);
    GameObject.Destroy(s.gameObject);
  }

  public static SkillSelectController get { get; set; }
}

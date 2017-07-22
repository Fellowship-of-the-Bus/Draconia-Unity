using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SkillSelectController: MonoBehaviour {

  public int NUM_EQUIPPED_SKILLS = 4;

  public GameObject skillInfo;
  public InvCharSelect charSelect;
  public Transform skillView;
  public Transform equippedSkillView;

  List<SkillInfo> equippedSkills  = new List<SkillInfo>();
  List<SkillInfo> nonEquippedSkills  = new List<SkillInfo>();
  List<SkillInfo> skills = new List<SkillInfo>();

  void Awake() {
    get = this;

    foreach(Type t in SkillList.get.skills) {
      GameObject o = Instantiate(skillInfo, skillView);
      SkillInfo s = o.GetComponent<SkillInfo>();
      s.init();
      s.info.GetComponent<Text>().text = t.FullName;
      s.skillType = t;
      skills.Add(s);
    }

  }

  public void setChar(Character c) {
    foreach (SkillInfo s in skills) {
      s.update(c.skills);
    }
  }

  public void back() {
    SceneManager.LoadSceneAsync ("OverWorld");
  }

  public bool equip(SkillInfo s) {
    if (!equippedSkills.Contains(s) && equippedSkills.Count < NUM_EQUIPPED_SKILLS) {
      s.transform.SetParent(equippedSkillView,false);
      nonEquippedSkills.Remove(s);
      equippedSkills.Add(s);
      return true;
    } else return false;
  }

  public void unequip(SkillInfo s) {
    s.transform.SetParent(skillView,false);
    nonEquippedSkills.Add(s);
    equippedSkills.Remove(s);
  }

  public static SkillSelectController get { get; set; }
}

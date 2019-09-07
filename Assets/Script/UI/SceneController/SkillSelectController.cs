using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SkillSelectController: MonoBehaviour {
  private const int NUM_EQUIPPED_SKILLS = 4;

  public GameObject skillInfo;
  public CharSelect charSelect;
  public Transform equippedSkillView;
  public GameObject skillTreeView;

  [Serializable]
  public struct NamedObject {
    public string name;
    public GameObject panel;
  }
  public NamedObject[] trees;

  List<SkillInfo> equippedSkills  = new List<SkillInfo>();
  List<SkillInfo> skills = new List<SkillInfo>();

  Character curChar;

  void Awake() {
    get = this;
    skills = new List<SkillInfo>(skillTreeView.GetComponentsInChildren<SkillInfo>(true));
    foreach (SkillInfo skill in skills) {
      skill.init(null, false);
    }
  }

  public void setTree(string treeName) {
    foreach (NamedObject treePanel in trees) {
      if (treeName == treePanel.name) {
        if (treePanel.panel) {
          treePanel.panel.SetActive(true);
        }
      } else {
        if (treePanel.panel) {
          treePanel.panel.SetActive(false);
        }
      }
    }
  }

  public void setChar(Character c) {
    curChar = c;
    foreach(SkillInfo s in equippedSkills) {
      GameObject.Destroy(s.gameObject);
    }
    equippedSkills.Clear();
    foreach (SkillInfo s in skills) {
      s.update(c);
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
      s.update(curChar);
      equippedSkills.Add(s);
      return s;
    }
    return null;
  }

  public void unequip(SkillInfo s) {
    equippedSkills.Remove(s);
    GameObject.Destroy(s.gameObject);
  }

  //Debug character level up
  public void levelUp() {
    if(curChar != null) {
      curChar.setLevelUp();
      charSelect.updateAttrView();
    }
  }

  public static SkillSelectController get { get; set; }
}

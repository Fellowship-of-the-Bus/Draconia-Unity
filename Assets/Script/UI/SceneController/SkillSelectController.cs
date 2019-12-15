using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SkillSelectController: MonoBehaviour {
  private const int NUM_EQUIPPED_SKILLS = 4;

  public CharSelect charSelect;

  // Prefabs to populate content
  public GameObject buttonPrefab;
  public GameObject skillTreePrefab;
  public GameObject skillRowPrefab;
  public GameObject skillInfoPrefab;

  // Panes to parent Content
  public Transform skillTreeView;
  public Transform treeSelectView;
  public Transform equippedSkillView;

  List<SkillInfo> equippedSkills = new List<SkillInfo>();

  List<GameObject> treeViews = new List<GameObject>();
  List<SkillInfo> skills = new List<SkillInfo>();

  Character curChar;

  void Awake() {
    get = this;

    foreach(KeyValuePair<String, Type[][]> entry in SkillList.skillsByClass) {
      // Setup skill tree display
      GameObject treeView = Instantiate(skillTreePrefab, skillTreeView);
      treeViews.Add(treeView);
      treeView.SetActive(false);

      // Setup tab button
      GameObject tab = Instantiate(buttonPrefab, treeSelectView);
      tab.GetComponentInChildren<Text>().text = entry.Key;
      Button tabButton = tab.GetComponent<Button>();
      tabButton.onClick.AddListener(() => {
        setTree(treeView);
      });
      int tierLevel = 0;

      // Populate skill tree display
      foreach(Type[] skillTier in entry.Value) {
        GameObject skillRow = Instantiate(skillRowPrefab, treeView.transform);

        foreach(Type skillType in skillTier) {
          GameObject skillInfo = Instantiate(skillInfoPrefab, skillRow.transform);
          SkillInfo skill = skillInfo.GetComponent<SkillInfo>();
          skill.init(skillType, false, tierLevel, entry.Key);
          skills.Add(skill);
        }
        tierLevel++;
      }
    }

    treeViews[0].SetActive(true);
  }

  public void setTree(GameObject newTreeView) {
    foreach (GameObject treeView in treeViews) {
      treeView.SetActive(false);
    }
    newTreeView.SetActive(true);
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
      GameObject o = Instantiate(skillInfoPrefab, equippedSkillView);
      SkillInfo s = o.GetComponent<SkillInfo>();
      // TODO: tier and spec are not easily available here or necessary for already equipped skills
      // figure out a good way to avoid passing garbage here
      s.init(t, true, 0, "Berserker");
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

  public void recalculateTiers() {
    foreach (SkillInfo s in skills) {
      s.checkAvailability();
    }
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

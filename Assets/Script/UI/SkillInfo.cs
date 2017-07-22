using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SkillInfo: MonoBehaviour {
  public Text info;
  public Button levelUpButton;
  public Button equipButton;

  public Type skillType;
  public int skillLevel {
    get {return tree.getSkillLevel(skillType);}
    set {tree.setSkillLevel(skillType, value);}
  }
  public SkillTree tree;

  SkillSelectController controller;

  Text equipText;

  SkillInfo parent = null;
  List<SkillInfo> children = new List<SkillInfo>();

  public bool equipped { get; private set; }

  public void init(bool isEquipped = false) {
    controller = SkillSelectController.get;
    equipText = equipButton.GetComponentInChildren<Text>();
    equipped = isEquipped;
    if (equipped) equipText.text = "Unequip";
  }

  public void update(SkillTree t, SkillInfo caller = null) {
    tree = t;
    equipButton.gameObject.SetActive(tree.isActive(skillType));
    info.text = skillType.FullName + ", level " + skillLevel;
    foreach(SkillInfo s in children) {
      if (s != caller) s.update(tree);
    }
  }

  public void levelup() {
    skillLevel = skillLevel + 1;
    update(tree);
    if (parent) parent.update(tree, this);
  }

  public void removeChild(SkillInfo s) {
    children.Remove(s);
  }

  public void equip(bool state) {
    if (equipped) {
      tree.unequipSkill(skillType);
      controller.unequip(this);
      if (parent) parent.removeChild(this);
    } else {
      if (skillLevel < 1) return;
      SkillInfo s = controller.equip(skillType);
      if (s) {
        tree.equipSkill(skillType);
        s.parent = this;
        children.Add(s);
      }
    }
  }

  public void equip() {
    equip(equipped);
  }
}

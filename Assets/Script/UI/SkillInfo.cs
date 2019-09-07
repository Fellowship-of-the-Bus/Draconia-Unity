using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Reflection;

public class SkillInfo: MonoBehaviour {
  public Tooltip tooltip;
  public Button levelUpButton;
  public Button equipButton;
  public Image displayImage;

  public String skillName;
  public Type skillType;
  public int skillLevel {
    get {return tree.getSkillLevel(skillType);}
    set {tree.setSkillLevel(skillType, value);}
  }
  public SkillTree tree;
  public Text equipText;

  SkillSelectController controller;

  SkillInfo parent = null;
  List<SkillInfo> children = new List<SkillInfo>();

  public bool equipped { get; private set; }

  public void init(Type type, bool isEquipped) {
    if (!String.IsNullOrEmpty(skillName)) {
      skillType = Assembly.GetExecutingAssembly().GetType(skillName);
    } else {
      skillType = type;
    }

    controller = SkillSelectController.get;
    equipped = isEquipped;
    if (equipped) equipText.text = "U";

    displayImage.GetComponent<Image>().sprite = SkillList.get.skillImages[skillType];
  }

  public void update(SkillTree t, SkillInfo caller = null) {
    if (skillType == null) {
      Debug.Log(skillName);
      return;
    }
    tree = t;
    equipButton.gameObject.SetActive(tree.isActive(skillType));
    equipButton.interactable = skillLevel > 0;

    foreach(SkillInfo s in children) {
      if (s != caller) s.update(tree);
    }
  }

  public void levelup() {
    skillLevel = skillLevel + 1;
    update(tree);
    if (parent) parent.update(tree, this);
  }

  private void removeChild(SkillInfo s) {
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

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
  public int skillLevel;
  public SkillTree tree;

  SkillSelectController controller;

  Text equipText;

  public void init() {
    controller = SkillSelectController.get;
    equipText = equipButton.GetComponentInChildren<Text>();
  }

  public void update(SkillTree t) {
    tree = t;
    equipButton.gameObject.SetActive(tree.isActive(skillType));
    skillLevel = tree.getSkillLevel(skillType);
    equipped = tree.isEquipped(skillType);
    info.text = skillType.FullName + ", level " + skillLevel;
    equip(equipped);
  }

  public void levelup() {
    skillLevel++;
    tree.setSkillLevel(skillType, skillLevel);
    update(tree);
  }

  private bool equipped = false;

  public void equip(bool state) {
    equipped = state;
    if (equipped) {
      tree.equipSkill(skillType);
      if (controller.equip(this)) equipText.text = "Unequip";
    }
    else {
      tree.unequipSkill(skillType);
      controller.unequip(this);
      equipText.text = "Equip";
    }
    info.text = skillType.FullName + ", level " + skillLevel;
  }

  public void equip() {
    equipped = !equipped;
    equip(equipped);
  }
}

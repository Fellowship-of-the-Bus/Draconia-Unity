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

  public Type skillType;
  public int skillLevel {
    get {return tree.getSkillLevel(skillType);}
    set {tree.setSkillLevel(skillType, value);}
  }
  public SkillTree tree;
  public Text equipText;
  int skillTier;
  string skillSpec; // Specialization the skill falls under

  SkillSelectController controller;

  SkillInfo parent = null;
  List<SkillInfo> children = new List<SkillInfo>();
  Skill skillInstance;

  public bool equipped { get; private set; }

  public void init(Type type, bool isEquipped, int tier, string spec) {
    skillType = type;
    skillInstance = (Skill)Activator.CreateInstance(skillType);
    skillTier = tier;
    skillSpec = spec;

    controller = SkillSelectController.get;
    equipped = isEquipped;
    if (equipped) equipText.text = "U";

    displayImage.GetComponent<Image>().sprite = SkillList.get.skillImages[skillType];
  }

  public void update(Character newChar, SkillInfo caller = null) {
    tree = newChar.skills;
    equipButton.gameObject.SetActive(tree.isActive(skillType));
    equipButton.interactable = skillLevel > 0;
    checkAvailability();

    foreach(SkillInfo s in children) {
      if (s != caller) s.update(newChar);
    }
    skillInstance.character = newChar;
    skillInstance.level = skillLevel;
    tooltip.tiptext = skillInstance.tooltip;
  }

  public void checkAvailability() {
    levelUpButton.interactable = skillTier <= tree.getSpecializationTier(skillSpec);
  }

  public void levelup() {
    skillLevel = skillLevel + 1;
    skillInstance.level = skillLevel;

    update(skillInstance.character);
    controller.recalculateTiers();
    if (parent) parent.update(skillInstance.character, this);
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

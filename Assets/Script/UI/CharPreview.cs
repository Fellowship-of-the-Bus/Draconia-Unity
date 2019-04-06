using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;


public class CharPreview: MonoBehaviour {
  public Text charName;
  public Image background;
  public BattleCharacter character;
  public Transform content;
  public SkillButton[] skillButtons;

  public void init(BattleCharacter c) {
    character = c;
    List<ActiveSkill> skills = character.baseChar.skills.getActives(character);
    int i = 0;
    for (; i < skills.Count; ++i) {
      ActiveSkill skill = skills[i];
      skillButtons[i].tooltip.tiptext = skill.tooltip;
      skillButtons[i].image.sprite = SkillList.get.skillImages[skill.GetType()];
    }
    for (; i < skillButtons.Length; ++i) {
      skillButtons[i].button.interactable = false;
      skillButtons[i].image.enabled = false;
    }
    charName.text = character.name;
  }
}

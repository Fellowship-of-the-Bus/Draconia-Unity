using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;


public class CharPreview: MonoBehaviour {
  public Text charName;
  public Image background;
  public BattleCharacter c;
  public Transform content;
  public GameObject skillButton;

  public void init(BattleCharacter character) {
    c = character;
    List<ActiveSkill> skills = c.baseChar.skills.getActives(c);
    foreach (ActiveSkill skill in skills) {
      GameObject o = Instantiate(skillButton, content);
      o.GetComponentInChildren<Text>().text = skill.name;
      Tooltip tooltip = o.GetComponent<Tooltip>();
      tooltip.tiptext = skill.tooltip;
      o.transform.Find("Image").GetComponent<Image>().sprite = SkillList.get.skillImages[skill.GetType()];
    }
    charName.text = c.name;
  }
}

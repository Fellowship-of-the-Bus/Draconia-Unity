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
      Tooltip tooltip = o.GetComponent<Tooltip>();
      tooltip.tiptext = skill.tooltip;
    }
    charName.text = c.name;
  }
}

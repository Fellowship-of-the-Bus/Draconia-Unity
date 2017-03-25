using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;

public class BasicAttackAI : BaseAttackAI {
  public override void target() {
    ActiveSkill skill = owner.equippedSkills[0];
    GameManager.get.SelectedSkill = 0;
    List<GameObject> targets = skill.getTargets();
    if (targets.Count == 0) return;
    List<Character> c = new List<Character>(targets.Take(1).Select(x => x.GetComponent<Character>()));
    c = new List<Character>(c.Filter((character) => character.team != owner.team));
    List<Effected> e = new List<Effected>(c.Map(x => x as Effected));
    owner.attackWithSkill(skill, e);
  }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;

public class BasicAttackAI : BaseAttackAI {
  public override void target() {
    ActiveSkill skill = owner.equippedSkills[0];
    List<GameObject> targets = skill.getTargets();
    if (targets.Count == 0) return;
    //todo aoe skills
    skill.activate(new List<Character>(targets.Take(1).Select(x => x.GetComponent<Character>())));
  }
}

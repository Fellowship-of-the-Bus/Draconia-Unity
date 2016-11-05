using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;

public class BasicAttackAI : BaseAttackAI {
  public override int getAttackRange() {
    return owner.equippedSkills[0].range;
  }

  public override void target(List<GameObject> targets) {
    if (targets.Count == 0) {
      return;
    } else {
      //todo aoe skills
      ActiveSkill skill = owner.equippedSkills[0];
      List<Character> characters = new List<Character>();
      characters.Add(targets[0].GetComponent<Character>());
      skill.activate(characters);
    }
  }
}

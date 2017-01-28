using UnityEngine;
using System.Collections.Generic;

public class SkillTree {
  Character self;

  public SkillTree(Character c) {
    self = c;
  }
  //just return something for testing for now
  public List<PassiveSkill> getPassives() {
    List<PassiveSkill> passives = new List<PassiveSkill>();
    PassiveSkill test = new StrengthBonus();
    test.level = 1;
    test.self = self;
    passives.Add(test);

    test = new FreezingHit();
    test.level = 1;
    test.self = self;
    passives.Add(test);

    return passives;
  }
}



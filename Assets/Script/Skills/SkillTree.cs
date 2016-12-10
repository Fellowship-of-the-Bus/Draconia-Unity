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

//    test = new Berserk();
//    test.level = 1;
//    test.self = self;
//    passives.Add(test);

    test = new Climb();
    test.level = 1;
    test.self = self;
    passives.Add(test);

//    test = new IronSkin();
//    test.level = 5;
//    test.self = self;
//    passives.Add(test);

//    test = new Vengeance();
//    test.level = 1;
//    test.self = self;
//    passives.Add(test);

//    test = new Dodge();
//    test.level = 7;
//    test.self = self;
//    passives.Add(test);

//    test = new CounterSkill();
//    test.level = 7;
//    test.self = self;
//    passives.Add(test);

//    test = new Adrenaline();
//    test.level = 1;
//    test.self = self;
//    passives.Add(test);

    return passives;
  }
}



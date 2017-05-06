using UnityEngine;
using System.Collections.Generic;

public interface Skill {
  int level {get; set;}
  BattleCharacter self {get; set;}
  void activate(BattleCharacter target);

  int Range {get; set;}
  bool useLos {get; set;}
  string name {get; set;}
}

using UnityEngine;
using System.Collections.Generic;

public interface Skill {
  int level {get; set;}
  BattleCharacter self {get; set;}
  Character character {get; set;} // used out of combat
  void activate(BattleCharacter target);

  int range {get; set;}
  bool useLos {get; set;}
  string name {get; set;}
  string tooltip {get;}
  string tooltipDescription {get;}
}

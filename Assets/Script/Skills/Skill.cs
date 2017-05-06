using UnityEngine;
using System.Collections.Generic;

public interface Skill {
  int level {get; set;}
  Character self {get; set;}
  void activate(Character target);

  int range {get; set;}
  bool useLos {get; set;}
  string name {get; set;}
}

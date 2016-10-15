using UnityEngine;
using System.Collections.Generic;

public abstract class Skill {
  public int id;
  public int level;
  public Character self;
  public abstract void activate(List<Character> targets);
  public abstract List<GameObject> getTargets();

  public int range {get; set;}
  public bool useLos {get; set;}
}

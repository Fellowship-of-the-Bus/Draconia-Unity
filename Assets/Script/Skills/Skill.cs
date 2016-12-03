using UnityEngine;
using System.Collections.Generic;

public abstract class Skill {
  public int level;
  public Character self;
  public abstract void activate(Character target);
  public abstract List<GameObject> getTargets();

  public int range {get; set;}
  public bool useLos {get; set;}
  public string name {get; set;}
}

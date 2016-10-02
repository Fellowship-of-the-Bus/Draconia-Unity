using UnityEngine;
using System.Collections.Generic;

public abstract class Skill : MonoBehaviour {
  public int id;
  public int level;
  public Character self;
  public abstract void activate(List<Character> targets);
  public abstract List<GameObject> getTargets();
}
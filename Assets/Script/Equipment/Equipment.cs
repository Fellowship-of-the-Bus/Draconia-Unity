using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public abstract class Equipment {
  string[] strs = {"Bronze", "Iron", "Steel", "Silver", "Gold", "Enchanted"};
  public List<string> tierName;
  public Attributes attr = new Attributes();
  public int tier;
  public string type;

  public void init() {
    tierName = new List<string>(strs);
  }
  public string name() {
    return tierName[tier] + " " + type;
  }

  public abstract void upgrade();
  public bool canUpgrade() {
    return tier < 5;
  }
}

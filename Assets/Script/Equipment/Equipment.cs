using UnityEngine;
using System.Collections.Generic;


[System.Serializable]
public abstract class Equipment {
  string[] tierName = new string[]{"Bronze", "Iron", "Steel", "Silver", "Gold", "Enchanted"};
  public Attributes attr = new Attributes();
  public int tier;
  public string equipmentClass;
  //could be null if not equipped.
  public Character equippedTo;

  public string name() {
    return tierName[tier] + " " + equipmentClass;
  }

  public abstract void upgrade();
  public bool canUpgrade() {
    return tier < 5;
  }

  public EquipType type;
}

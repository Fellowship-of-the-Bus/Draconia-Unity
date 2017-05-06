using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Inventory {
  public List<Equipment> inv = new List<Equipment>();

  public void combine(Equipment b, Equipment e, Equipment e2) {
    //check same tier
    if (b.tier != e.tier || b.tier != e2.tier) {
      return;
    }
    if (b.canUpgrade()) {
      inv.Remove(e);
      inv.Remove(e2);
      b.upgrade();
    }
  }
}

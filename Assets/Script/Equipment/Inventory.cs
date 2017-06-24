using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Inventory {
  public List<Equipment> equipments = new List<Equipment>();

  public void combine(Equipment b, Equipment e, Equipment e2) {
    //check same tier
    if (b.tier != e.tier || b.tier != e2.tier) {
      return;
    }
    if (b.canUpgrade()) {
      equipments.Remove(e);
      equipments.Remove(e2);
      b.upgrade();
    }
  }
}

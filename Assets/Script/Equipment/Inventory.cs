using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Inventory {
  public LinkedList<Equipment> equipments = new LinkedList<Equipment>();

  public Equipment combine(Equipment b, Equipment e, Equipment e2) {
    //check same tier
    if (b.tier != e.tier || b.tier != e2.tier) {
      return null;
    }
    if (b.canUpgrade()) {
      return b.upgrade(e, e2);
    }
    return null;
  }
  public void deleteEquipment(Equipment e) {
    equipments.Remove(e);
  }

  public void addEquipment(Equipment e) {
    equipments.AddLast(e);
  }
}

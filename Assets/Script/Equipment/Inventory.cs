using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Inventory {
  public LinkedList<Equipment> equipments = new LinkedList<Equipment>();

  public void addEquipment(Equipment e) {
    equipments.AddLast(e);
  }
}

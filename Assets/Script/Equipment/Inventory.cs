using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Inventory {
  public List<Weapon> weapons = new List<Weapon>();
  public List<Armour> armour = new List<Armour>();

  public void addEquipment(Weapon e) {
    weapons.Add(e);
  }

  public void addEquipment(Armour e) {
    armour.Add(e);
  }
}

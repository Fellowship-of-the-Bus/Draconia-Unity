using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Weapon : Equipment {
  public int range = 1;
  public enum kinds { Sharp, Blunt, Ranged };
  public kinds kind = kinds.Sharp;

  public override void upgrade() {

  }
  /*public Weapon(kinds k) {
    range = k == Ranged ? 3 : 1;
    kind = k;
  }*/
}

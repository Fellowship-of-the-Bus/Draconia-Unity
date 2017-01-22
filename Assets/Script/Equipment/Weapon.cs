using UnityEngine;
using System.Collections.Generic;

public class Weapon : MonoBehaviour {
  public int range = 1;
  public enum kinds { Sharp, Blunt, Ranged };
  public kinds kind = kinds.Sharp;

  /*public Weapon(kinds k) {
    range = k == Ranged ? 3 : 1;
    kind = k;
  }*/
}

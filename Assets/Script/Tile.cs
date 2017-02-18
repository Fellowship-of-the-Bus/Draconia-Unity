using UnityEngine;
using System.Collections.Generic;

public class Tile : Effected {
  public int distance = 0;
  public int movePointSpent = 1;
  public Vector3 dir = Vector3.zero;
  public GameObject occupant = null;
  public float additionalHeight = 0f;

  public bool occupied() {
    return occupant != null;
  }

  public float getHeight() {
    return gameObject.transform.localScale.y/2 + additionalHeight;
  }
}

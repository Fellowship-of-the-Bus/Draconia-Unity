using UnityEngine;
using System.Collections.Generic;

public enum Direction {
  Forward,
  Right,
  Back,
  Left,
  None
}

public class Tile : Effected {
  public int distance = 0;
  public int movePointSpent = 1;
  public Direction dir = Direction.None;
  public GameObject occupant = null;
  public float additionalHeight = 0f;

  public bool occupied() {
    return occupant != null;
  }

  public float getHeight() {
    return gameObject.transform.localScale.y/2 + additionalHeight;
  }
}

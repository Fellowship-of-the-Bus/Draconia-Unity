using UnityEngine;

public enum Direction {
  Forward,
  Right,
  Back,
  Left,
  None
}

public class Tile : MonoBehaviour {
  public int distance = 0;
  public int movePointSpent = 1;
  public Direction dir = Direction.None;
}

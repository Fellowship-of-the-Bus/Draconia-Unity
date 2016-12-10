using UnityEngine;
using System.Collections.Generic;

public class Aura<T> : Effect where T: Effect {
  public T effect;
  public int radius;
  private List<Character> affected = new List<Character>();

  public override void onActivate() {
    List<Tile> tiles = GameManager.get.getTilesWithinRange(owner.curTile, radius);

    affected.Add(owner);
    foreach (Tile t in tiles) {
      Character c = t.occupant.GetComponent<Character>();
      if (t.occupied() && c.team == owner.team) {
        affected.Add(c);
        c.applyEffect(effect);
      }
    }
  }

  public override void onDeactivate() {
    foreach (Character c in affected) {
      c.removeEffect(effect);
    }
  }
}

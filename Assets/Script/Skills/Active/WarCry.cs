using UnityEngine;
using System.Collections.Generic;

public class WarCry: ActiveSkill, AoeSkill {
  public int aoe {get; set;}

  public WarCry() {
    range = 0;
    aoe = 1;
    useLos = false;
    name = "WarCry";
  }

  public override void additionalEffects (Character target) {
    WarCryEffect e = new WarCryEffect();
    e.level = level;
    target.applyEffect(e);
  }

  public override List<GameObject> getTargets() {
    List<GameObject> targets = new List<GameObject>();
    targets.Add(self.curTile.gameObject);
    return targets;
  }

  public List<GameObject> getTargetsInAoe(Vector3 position) {
    List<Tile> tiles = GameManager.get.getTilesWithinRange(self.curTile, aoe);
    List<GameObject> targets = new List<GameObject>();
    targets.Add(self.gameObject);
    foreach (Tile t in tiles) {
      if (t.occupied() && (t.occupant.GetComponent<Character>().team == self.team)) {
        targets.Add(t.occupant);
      }
    }
    return targets;
  }

  public override int calculateDamage(Character source, Character target) {
    return 0;
  }


}
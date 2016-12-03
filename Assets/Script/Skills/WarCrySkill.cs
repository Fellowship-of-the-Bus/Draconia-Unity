using UnityEngine;
using System.Collections.Generic;

public class WarCrySkill: ActiveSkill, AoeSkill {
  public int aoe {get; set;}

  public WarCrySkill() {
    range = 1;
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
      if (t.occupied()) {
        targets.Add(t.occupant.gameObject);
      }
    }
    return targets;
  }

  public override int calculateDamage(Character source, Character target) {
    return 0;
  }


}

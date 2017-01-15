using UnityEngine;
using System.Collections.Generic;

public class IgniteWeapon: ActiveSkill {
  public IgniteWeapon() {
    range = 1;
    useLos = false;
    name = "IgniteWeapon";
  }

  public override List<GameObject> getTargets() {
    List<Tile> tiles = GameManager.get.getTilesWithinRange(self.curTile, 1);
    List<GameObject> targets = new List<GameObject>();
    foreach (Tile t in tiles) {
      if (t.occupied()) {
        targets.Add(t.occupant);
      }
    }
    return targets;
  }


  public override int calculateDamage(Character source, Character target) {
    return 0;
  }

  public override void additionalEffects (Character target) {
    IgniteWeaponEffect buff = new IgniteWeaponEffect();
    buff.level = level;
    buff.duration = 2;
    target.applyEffect(buff);
  }

}

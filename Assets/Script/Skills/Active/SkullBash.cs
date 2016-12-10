using UnityEngine;
using System.Collections.Generic;
using System;

public class SkullBash: ActiveSkill {
  public SkullBash() {
    range = 1;
    useLos = false;
    name = "Skull Bash";
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
    return (int)(source.attr.strength*(0.5+level*0.05) - target.attr.physicalDefense);
  }

  public override void additionalEffects (Character target) {
    DazedEffect debuff = new DazedEffect();
    target.curAction = Math.Max(0, target.curAction - 300);
    debuff.level = level;
    debuff.duration = 3;
    target.applyEffect(debuff);
    ActionQueue.get.updateTime(target.gameObject);
  }

}

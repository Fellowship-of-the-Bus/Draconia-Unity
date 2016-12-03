﻿using UnityEngine;
using System.Collections.Generic;

public class CrippleSkill: ActiveSkill {
  public CrippleSkill() {
    range = 1;
    useLos = false;
    name = "Cripple";
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
    CrippleEffect debuff = new CrippleEffect();
    debuff.level = level;
    target.applyEffect(debuff);
  }

}

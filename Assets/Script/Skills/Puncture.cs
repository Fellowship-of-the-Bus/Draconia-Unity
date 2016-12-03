using UnityEngine;
using System.Collections.Generic;

public class Puncture: ActiveSkill {
  public Puncture() {
    range = 1;
    useLos = false;
    name = "Puncture";
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
    BleedEffect debuff = new BleedEffect();
    debuff.level = level;
    debuff.duration = (level+5)/2;
    debuff.damage = (int)System.Math.Max((int)calculateDamage(self, target)*(0.2f + 0.1f*level), 1);
    target.applyEffect(debuff);
  }

}

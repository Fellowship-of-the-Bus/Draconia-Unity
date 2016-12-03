using UnityEngine;
using System.Collections.Generic;

public class CrippleSkill: ActiveSkill {
  public CrippleSkill() {
    range = 1;
    useLos = false;
    name = "Cripple";
  }
  public override void activate(List<Character> targets) {
    CrippleEffect debuff = new CrippleEffect();
    debuff.level = level;

    foreach (Character c in targets) {
      c.takeDamage(calculateDamage(self, c));
      c.applyEffect(debuff);
    }
  }
  public override List<GameObject> getTargets() {
    List<Tile> tiles = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().getTilesWithinRange(self.curTile, 1);
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


}

using UnityEngine;
using System.Collections.Generic;

public class ShareLife: CircleAoE, HealingSkill {
  public ShareLife() {
    range = 3;
    useWepRange = false;
    aoe = 2;
    useLos = false;
    name = "Share Life";
    effectsTiles = false;
    maxCooldown = 2;
    targetAlly(true);
    targetEnemy(false);
  }

  public override BattleCharacter self {
    set {
      base.self = value;
      attachListener(self, EventHook.postSkill);
    }
  }

  protected override void trigger(Event e) {
    if (e.hook == EventHook.postSkill && e.skillUsed == this) {
      if ((int)(self.intelligence*(1+level*0.1)) >= self.curHealth) {
        if (self.curHealth > 1) self.takeDamage(self.curHealth - 1);
      }
      else self.takeDamage((int)(self.intelligence*(1+level*0.1)));
    }
  }

  public int healingFormula() {
    int amount = (int)(self.intelligence*(1+level*0.1));
    if (amount >= self.curHealth) amount = self.curHealth - 1;
    return amount;
  }

  public override List<Tile> getTargetsInAoe(Vector3 position) {
    List<Tile> l = base.getTargetsInAoe(position);
    l.Remove(self.curTile);
    List<Tile> l2 = new List<Tile>(l.Filter(tile => tile.occupant == null || tile.occupant.team == self.team));
    return l2;
  }
}

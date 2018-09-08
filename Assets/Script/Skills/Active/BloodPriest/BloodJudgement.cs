using UnityEngine;
using System.Collections.Generic;

public class BloodJudgement: CircleAoE {
  int cost = 0;

  public override string animation { get { return "Cast"; }}
  public override Color castColor { get { return castColorBloodPriest; }}

  public BloodJudgement() {
    range = 3;
    useWepRange = false;
    aoe = 2;
    useLos = false;
    name = "Blood Judgement";
    effectsTiles = false;
    maxCooldown = 2;
    targetsTiles = true;
    dType = DamageType.magical;
    targetAlly(true);
    targetEnemy(true);
  }

  public override BattleCharacter self {
    set {
      base.self = value;
      attachListener(self, EventHook.postSkill);
      cost = self.maxHealth / 10;
    }
  }

  float percent() {
    if (self.curHealth <= cost) {
      return 1 - ((cost - self.curHealth + 1.0f) / cost);
    }
    return 1f;
  }

  protected override void trigger(Event e) {
    if (e.hook == EventHook.postSkill) {
      //Does not use calculate damage since the cost should not be effected by any defenses
      self.takeDamage((int)(cost * percent()));
    }
  }

  int amount() {
    return (int)(self.intelligence * 0.25f);
  }

  public override void additionalEffects (BattleCharacter target) {
    if (target == self) return;

    if (target.team == self.team) {
      target.takeHealing((int)(target.calculateHealing((int)(amount() * percent()))));
    } else {
      target.takeDamage((int)(target.calculateDamage((int)(amount() * percent()), DamageType.magical, DamageElement.none)));
    }
  }

  public override List<Tile> getTargetsInAoe(Vector3 position) {
    List<Tile> l = base.getTargetsInAoe(position);
    l.Remove(self.curTile);
    return l;
  }
}

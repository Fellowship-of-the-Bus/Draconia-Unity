using UnityEngine;
using System.Collections.Generic;

public class WarCry: CircleAoE {
  public WarCry() {
    range = 0;
    aoe = 3;
    useLos = false;
    name = "War Cry";
    effectsTiles = false;
    maxCooldown = 2;
    targetAlly(true);
    targetEnemy(false);
  }

  protected override string tooltipDescription { get {
    return "Let loose an inspiring cry, increasing the speed of all allies within range by 1";
  }}

  public override void additionalEffects (BattleCharacter target) {
    WarCryEffect e = new WarCryEffect();
    e.level = level;
    e.duration = 1;
    e.caster = self;
    target.applyEffect(e);
  }
}

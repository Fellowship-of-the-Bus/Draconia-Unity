using UnityEngine;
using System.Collections.Generic;

public class WarCry: CircleAoE {
  public WarCry() {
    range = 0;
    aoe = 3;
    useLos = false;
    name = "WarCry";
    effectsTiles = false;
    maxCooldown = 2;
    targetAlly(true);
    targetEnemy(false);
  }

  public override string tooltip { get { return "Let loose an inspiring cry, increasing the speed of all allies within 3 tiles by 1"; }}

  public override void additionalEffects (BattleCharacter target) {
    WarCryEffect e = new WarCryEffect();
    e.level = level;
    e.duration = 1;
    target.applyEffect(e);
  }
}

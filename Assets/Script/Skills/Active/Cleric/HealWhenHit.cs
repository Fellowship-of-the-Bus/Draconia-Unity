using UnityEngine;
using System.Collections.Generic;

public class HealWhenHit : CircleAoE {

  public override string animation { get { return "ClericCast"; }}
  public override Color castColor { get { return castColorCleric; }}

  public HealWhenHit() {
    range = 3;
    useWepRange = false;
    aoe = 3;
    useLos = false;
    name = "HealWhenHit";
    effectsTiles = false;
    maxCooldown = 2;
    targetAlly(true);
    targetEnemy(false);
  }

  protected override string tooltipDescription { get {
    return "Heal the target by " + tooltipHealing + " each time they take damage";
  }}

  public override void additionalEffects(BattleCharacter target) {
    HealWhenHitEffect e = new HealWhenHitEffect();
    e.duration = 5;
    e.level = level;
    e.caster = self;
    target.applyEffect(e);
  }
}

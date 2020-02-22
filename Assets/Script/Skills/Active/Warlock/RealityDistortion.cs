using System;
using UnityEngine;

public class RealityDistortion : SingleTarget {

  public override string animation { get { return "Cast"; }}
  public override Color castColor { get { return castColorWarlock; }}

  public RealityDistortion() {
    useWepRange = false;
    range = 5;
    useLos = false;
    name = "Reality Distortion";
    maxCooldown = 0;
    canTargetSelf = true;
    targetAlly(true);
    targetEnemy(true);
  }

  public override void additionalEffects (BattleCharacter target) {
    RealityDistortionEffect debuff = new RealityDistortionEffect();
    debuff.duration = 2;
    debuff.caster = self;
    target.applyEffect(debuff);
  }

  public override string tooltipDescription { get {
    return "Distort reality on the target, reversing the modifications made by stat changing effects for 2 turns";
  }}
}


using System;
using UnityEngine;

public class RealityDistortion : SingleTarget {

  public override string animation { get { return "Cast"; }}
  public override Color castColor { get { return castColorWarlock; }}

  public RealityDistortion() {
    useWepRange = true;
    useLos = false;
    name = "Reality Distortion";
    maxCooldown = 0;
    canTargetSelf = true;
    targetAlly(true);
    targetEnemy(true);
  }

  public override void additionalEffects (BattleCharacter target) {
    RealityDistortionEffect debuff = new RealityDistortionEffect();
    debuff.level = level;
    debuff.duration = 2;
    debuff.caster = self;
    target.applyEffect(debuff);
  }
}


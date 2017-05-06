using System;

public class RealityDistortion : SingleTarget {
  public RealityDistortion() {
    useWepRange = true;
    useLos = false;
    name = "Reality Distortion";
    maxCooldown = 0;
    canTargetSelf = true;
  }

  public override void additionalEffects (Character target) {
    RealityDistortionEffect debuff = new RealityDistortionEffect();
    debuff.level = level;
    debuff.duration = 2;
    target.applyEffect(debuff);
  }
}


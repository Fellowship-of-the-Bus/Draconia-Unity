using UnityEngine;

public class BleedEffect : DurationEffect, HealthChangingEffect {
  private GameObject particle;
  private static GameObject particlePrefab = null;
  public BleedEffect() {
    name = "Bleeding";
  }

  protected override string tooltipDescription { get {
    return "Taking " + effectValue + " damage at the end of each turn";
  }}

  protected override void onActivate() {
    attachListener(owner, EventHook.endTurn);

    if (particlePrefab == null) {
      particlePrefab = (GameObject)Resources.Load("ParticleEffects/Bleeding");
    }
    particle = owner.applyParticle(particlePrefab);
  }
  protected override void onDeactivateListeners() {
    detachListener(owner);
    owner.removeParticle(particle);
  }
  protected override void additionalEffect(Draconia.Event e) {
    owner.takeDamage(effectValue,caster);
  }

  public int healthChange() {
    return -effectValue;
  }

  // Get a preview of the total damage of an effect with the given parameters
  public static int totalDamage(int initialDamage, int duration) {
    return initialDamage * duration;
  }
}

using UnityEngine;

public class BleedEffect : DurationEffect, HealthChangingEffect {
  private GameObject particle;
  public BleedEffect() {
    name = "Bleeding";
  }

  protected override string tooltipDescription { get {
    return "Taking " + effectValue + " damage at the end of each turn";
  }}

  protected override void onActivate() {
    attachListener(owner, EventHook.endTurn);

    // TODO: See about loading this once for the game
    particle = owner.applyParticle((GameObject)Resources.Load("ParticleEffects/Bleeding"));
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

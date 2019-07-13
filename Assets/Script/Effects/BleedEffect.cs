using UnityEngine;

public class BleedEffect : DurationEffect, HealthChangingEffect {
  private GameObject particle;

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
}

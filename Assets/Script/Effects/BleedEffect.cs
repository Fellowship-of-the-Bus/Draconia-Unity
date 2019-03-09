using UnityEngine;

public class BleedEffect : DurationEffect, HealthChangingEffect {
  public int damage;
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
    owner.takeDamage(damage);
  }
  public override int CompareTo(Effect e) {
    Debug.Assert(e is BleedEffect);
    if (e is BleedEffect) {
      return this.damage.CompareTo((e as BleedEffect).damage);
    } else {
      //should only be comparing bleed effects, shouldn't ever get here.
      return base.CompareTo(e);
    }
  }

  public int healthChange() {
    return -damage;
  }
}

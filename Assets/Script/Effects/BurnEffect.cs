using UnityEngine;

public class BurnEffect : DurationEffect, HealthChangingEffect {
  public int damage;
  public float multiplier = 1.0f;
  private GameObject particle;

  protected override void onActivate() {
    attachListener(owner, EventHook.endTurn);

    // TODO: See about loading this once for the game
    particle = owner.applyParticle((GameObject)Resources.Load("ParticleEffects/Burning"));
  }
  protected override void onDeactivateListeners() {
    detachListener(owner);
    owner.removeParticle(particle);
  }
  protected override void additionalEffect(Draconia.Event e) {
    owner.takeDamage(owner.calculateDamage((int)(damage * multiplier), DamageType.none, DamageElement.fire));
    multiplier -= 0.15f;
  }
  public override int CompareTo(Effect e) {
    Debug.Assert(e is BurnEffect);
    if (e is BurnEffect) {
      return this.damage.CompareTo((e as BurnEffect).damage);
    } else {
      //should only be comparing Burn effects, shouldn't ever get here.
      return base.CompareTo(e);
    }
  }

  public int healthChange() {
    return -(int)(damage * multiplier);
  }
}

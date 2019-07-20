using UnityEngine;

public class PoisonEffect : DurationEffect, HealthChangingEffect {
  public float multiplier = 1.0f;
  private GameObject particle;

  protected override void onActivate() {
    attachListener(owner, EventHook.endTurn);

    // TODO: See about loading this once for the game
    particle = owner.applyParticle((GameObject)Resources.Load("ParticleEffects/Poison"));
  }
  protected override void onDeactivateListeners() {
    detachListener(owner);
    owner.removeParticle(particle);
  }
  protected override void additionalEffect(Draconia.Event e) {
    owner.takeDamage((int)(effectValue * multiplier),caster);
    multiplier += 0.15f;
  }
  public override int CompareTo(Effect e) {
    Debug.Assert(e is PoisonEffect);
    if (e is PoisonEffect) {
      return (effectValue*multiplier).CompareTo((e as PoisonEffect).effectValue*(e as PoisonEffect).multiplier);
    } else {
      //should only be comparing Poison effects, shouldn't ever get here.
      return base.CompareTo(e);
    }
  }

  public int healthChange() {
    return -(int)(effectValue * multiplier);
  }
}

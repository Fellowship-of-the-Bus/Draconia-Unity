using UnityEngine;

public class BurnEffect : DurationEffect, HealthChangingEffect {
  const float multiplierReduction = 0.15f;
  const float baseMultiplier = 1.0f;
  public float multiplier = baseMultiplier;
  private GameObject particle;

  public BurnEffect() {
    name = "On Fire!";
  }

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
    Debug.Assert(duration < 7);
    owner.takeDamage(owner.calculateDamage((int)(effectValue * multiplier), DamageType.none, DamageElement.fire),caster);
    multiplier -= multiplierReduction;
  }

  public override int CompareTo(Effect e) {
    Debug.Assert(e is BurnEffect);
    if (e is BurnEffect) {
      return (this.effectValue*multiplier).CompareTo((e as BurnEffect).effectValue*(e as BurnEffect).multiplier);
    } else {
      //should only be comparing Burn effects, shouldn't ever get here.
      return base.CompareTo(e);
    }
  }

  public int healthChange() {
    return -(int)(effectValue * multiplier);
  }

  // Get a preview of the total damage of an effect with the given parameters
  public static int totalDamage(int initialDamage, int duration) {
    int accumulatedDamage = 0;
    for (int i = 0; i < duration; i++) {
      accumulatedDamage += (int)(initialDamage * (baseMultiplier - (multiplierReduction * i)));
    }

    return accumulatedDamage;
  }
}

using UnityEngine;
using System.Collections.Generic;

public class ScorchEarthEffect : DurationEffect, HealthChangingEffect {
  List<BattleCharacter> effected = new List<BattleCharacter>();
  GameObject particleEff;

  protected override void onActivate() {
    attachListener(GameManager.get.eventManager, EventHook.endTurn);
    attachListener(GameManager.get.eventManager, EventHook.enterTile);
    attachListener(caster, EventHook.endTurn);

    particleEff = (GameObject) GameObject.Instantiate(Resources.Load("ParticleEffects/Scorched Earth"),
      new Vector3(ownerTile.transform.position.x,
        ownerTile.transform.position.y + ownerTile.getHeight(),
        ownerTile.transform.position.z), Quaternion.identity);
  }

  protected override void onDeactivateListeners() {
    detachListener(GameManager.get.eventManager);
    detachListener(caster);
  }

  protected override void onDeactivateEffects() {
    Object.Destroy(particleEff);
  }

  float damage(BattleCharacter c) {
    return c.calculateDamage(effectValue, DamageType.none, DamageElement.fire);
  }

  protected override void additionalEffect(Draconia.Event e) {
    if (e.sender == null && e.hook == EventHook.endTurn) {
      BattleCharacter occupant = ownerTile.occupant;
      if (occupant != null && occupant == e.endTurnChar && !(effected.Contains(occupant)) && !occupant.levitating) {
        GameManager.get.waitFor(0.5f, () => occupant.takeDamage((int)(damage(occupant)),caster)); //TODO: animation time rather than 0.5f
      }
      effected.Clear();
    } else if (e.hook == EventHook.enterTile && e.position == ownerTile.transform.position  && !(e.sender.levitating)) {
      effected.Add(e.sender);
      GameManager.get.waitFor(0.5f, () => e.sender.takeDamage((int)(damage(e.sender)),caster)); //TODO: animation time rather than 0.5f
      e.interruptMove = true;
    }
  }

  public override bool shouldDecrement(Draconia.Event e) {
    return e.sender == caster;
  }

  public int healthChange() {
    if (ownerTile.occupant == null) return 0;
    if (effected.Contains(ownerTile.occupant)) return 0;
    return -(int)damage(ownerTile.occupant);
  }
}

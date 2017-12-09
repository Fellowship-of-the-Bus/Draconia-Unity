using UnityEngine;
using System.Collections.Generic;

public class ScorchEarthEffect : DurationEffect {
  public BattleCharacter caster;
  List<BattleCharacter> effected = new List<BattleCharacter>();
  protected override void onActivate() {
    attachListener(GameManager.get.eventManager, EventHook.endTurn);
    attachListener(GameManager.get.eventManager, EventHook.enterTile);
    attachListener(caster, EventHook.endTurn);
  }

  protected override void onDeactivateListeners() {
    detachListener(GameManager.get.eventManager);
    detachListener(caster);
  }

  float damage(BattleCharacter c) {
    return c.calculateDamage((int)(caster.intelligence * 0.2f), DamageType.none, DamageElement.fire);
  }

  protected override void additionalEffect(Event e) {
    if (e.sender == null && e.hook == EventHook.endTurn) {
      BattleCharacter occupant = null;
      if (ownerTile.occupant != null) {
        occupant = ownerTile.occupant.GetComponent<BattleCharacter>();
      }
      if (occupant != null && occupant == e.endTurnChar && !(effected.Contains(occupant)) && !(occupant.levitating)) {
        GameManager.get.waitFor(0.5f, () => occupant.takeDamage((int)(damage(occupant)))); //TODO: animation time rather than 0.5f
      }
      effected.Clear();
    } else if (e.hook == EventHook.enterTile && e.position == ownerTile.transform.position  && !(e.sender.levitating)) {
      effected.Add(e.sender);
      GameManager.get.waitFor(0.5f, () => e.sender.takeDamage((int)(damage(e.sender)))); //TODO: animation time rather than 0.5f
      e.interruptMove = true;
    }
  }

  public override bool shouldDecrement(Event e) {
    return e.sender == caster;
  }
}

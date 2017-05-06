using UnityEngine;
using System.Collections.Generic;

public class BloodSanctuaryEffect : DurationEffect {
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

  float healing(BattleCharacter c) {
    return caster.intelligence * 1f * c.healingMultiplier;
  }

  protected override void additionalEffect(Event e) {
    if (e.sender == null && e.hook == EventHook.endTurn) {
      BattleCharacter occupant = null;
      if (ownerTile.occupant != null) {
        occupant = ownerTile.occupant.GetComponent<BattleCharacter>();
      }
      if (occupant != null && occupant == e.endTurnChar && !(effected.Contains(occupant))  && !(occupant.levitating)) {
        occupant.takeHealing((int)(healing(occupant)));
      }
      effected.Clear();
    } else if (e.hook == EventHook.enterTile && e.position == ownerTile.transform.position && !(e.sender.levitating)) {
      effected.Add(e.sender);
      e.sender.takeHealing((int)(healing(e.sender)));
    }
  }

  public override bool shouldDecrement(Event e) {
    return e.sender == caster;
  }
}

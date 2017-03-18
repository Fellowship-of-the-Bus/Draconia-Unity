using UnityEngine;
using System.Collections.Generic;

public class ScorchEarthEffect : DurationEffect {
  public Character caster;
  List<Character> effected = new List<Character>();
  protected override void onActivate() {
    attachListener(GameManager.get.eventManager, EventHook.endTurn);
    attachListener(GameManager.get.eventManager, EventHook.enterTile);
    attachListener(caster, EventHook.endTurn);
  }

  protected override void onDeactivateListeners() {
    detachListener(GameManager.get.eventManager);
    detachListener(caster);
  }

  float damage(Character c) {
    return c.calculateDamage((int)(caster.intelligence * 0.2f), DamageType.none, DamageElement.fire);
  }

  protected override void additionalEffect(Event e) {
    if (e.sender == null && e.hook == EventHook.endTurn) {
      Character occupant = null;
      if (ownerTile.occupant != null) {
        occupant = ownerTile.occupant.GetComponent<Character>();
      }
      if (occupant != null && occupant == e.endTurnChar && !(effected.Contains(occupant)) && !(occupant.levitating)) {
        occupant.takeDamage((int)(damage(occupant)));
      }
      effected.Clear();
    } else if (e.hook == EventHook.enterTile && e.position == ownerTile.transform.position  && !(e.sender.levitating)) {
      effected.Add(e.sender);
      e.sender.takeDamage((int)(damage(e.sender)));
    }
  }

  public override bool shouldDecrement(Event e) {
    return e.sender == caster;
  }
}

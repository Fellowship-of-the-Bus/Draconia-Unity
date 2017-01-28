using UnityEngine;

public class ScorchEarthEffect : DurationEffect {
  public Character caster;
  public override void onActivate() {
    attachListener(GameManager.get.eventManager, EventHook.endTurn);
    attachListener(caster, EventHook.endTurn);
  }

  public override void onDeactivateListeners() {
    detachListener(GameManager.get.eventManager);
    detachListener(caster);
  }

  public override void additionalEffect(Event e) {
    if (e.sender == null && e.hook == EventHook.endTurn) {
      Character occupant = null;
      if (ownerTile.occupant != null) {
        occupant = ownerTile.occupant.GetComponent<Character>();
      }
      if (occupant != null && occupant == e.endTurnChar){
        occupant.takeDamage((int)(2*occupant.fireResMultiplier));
      }
    }
  }
}

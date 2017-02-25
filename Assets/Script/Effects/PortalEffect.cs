using UnityEngine;
using System.Collections.Generic;

public class PortalEffect : DurationEffect {
  GameObject block;
  public Character caster;
  public PortalEffect sibling;

  public List<Character> effected = new List<Character>();
  protected override void onActivate() {
    attachListener(GameManager.get.eventManager, EventHook.endTurn);
    attachListener(GameManager.get.eventManager, EventHook.enterTile);
    attachListener(caster, EventHook.endTurn);
    block = (GameObject) GameObject.Instantiate(GameManager.get.iceBlock, new Vector3(ownerTile.gameObject.transform.position.x, ownerTile.gameObject.transform.position.y + ownerTile.getHeight() + 0.5f, ownerTile.gameObject.transform.position.z), Quaternion.identity);
  }

  protected override void onDeactivateListeners() {
    detachListener(GameManager.get.eventManager);
    detachListener(caster);
  }

  protected override void additionalEffect(Event e) {
    if (sibling.ownerTile.occupied()) return;

    Character occupant = ownerTile.occupant != null ? ownerTile.occupant.GetComponent<Character>() : e.sender;

    if (e.sender == null && e.hook == EventHook.endTurn) {
      effected.Clear();
    } else if (e.hook == EventHook.enterTile && e.position == ownerTile.transform.position  && !(occupant.levitating)) {
      if (effected.Contains(occupant)) return;
      GameManager.get.MovePiece(occupant, sibling.ownerTile);
      effected.Add(occupant);
    }
  }

  protected override void onDeactivateEffects() {
    if (ownerTile.occupant) ownerTile.occupant.transform.position = new Vector3(ownerTile.occupant.transform.position.x, ownerTile.gameObject.transform.position.y + ownerTile.getHeight() + 0.5f, ownerTile.occupant.transform.position.z);
    Object.Destroy(block);
    detachListener(caster);
  }

  public override bool shouldDecrement(Event e) {
    return e.sender == caster;
  }
}

using UnityEngine;
using System.Collections.Generic;

public class PortalEffect : DurationEffect {
  GameObject block;
  public BattleCharacter caster;
  public PortalEffect sibling;

  public List<BattleCharacter> effected = new List<BattleCharacter>();
  protected override void onActivate() {
    attachListener(caster, EventHook.endTurn);
    block = (GameObject) GameObject.Instantiate(GameManager.get.iceBlock, new Vector3(ownerTile.gameObject.transform.position.x, ownerTile.gameObject.transform.position.y + ownerTile.getHeight() + 0.5f, ownerTile.gameObject.transform.position.z), Quaternion.identity,
      ownerTile.transform);
  }

  protected override void onDeactivateListeners() {
    detachListener(caster);
  }

  protected override void onDeactivateEffects() {
    Object.Destroy(block);
    detachListener(caster);
  }
}

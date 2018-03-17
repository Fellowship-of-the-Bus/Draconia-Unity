using UnityEngine;
using System.Collections.Generic;

public class PortalEffect : DurationEffect {
  GameObject block;
  public BattleCharacter caster;
  public PortalEffect sibling;

  public List<BattleCharacter> effected = new List<BattleCharacter>();
  protected override void onActivate() {
    attachListener(caster, EventHook.endTurn);
    block = (GameObject) GameObject.Instantiate(Resources.Load("Map/Portal"),
      new Vector3(ownerTile.transform.position.x,
        ownerTile.transform.position.y + ownerTile.getHeight() + 0.5f,
        ownerTile.transform.position.z), Quaternion.identity);
  }

  protected override void onDeactivateListeners() {
    detachListener(caster);
  }

  protected override void onDeactivateEffects() {
    Object.Destroy(block);
    detachListener(caster);
  }
}

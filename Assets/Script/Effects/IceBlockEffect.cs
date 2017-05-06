using UnityEngine;

public class IceBlockEffect : DurationEffect {
  GameObject block;
  public BattleCharacter caster;
  protected override void onActivate() {
    block = (GameObject) GameObject.Instantiate(GameManager.get.iceBlock, new Vector3(ownerTile.gameObject.transform.position.x, ownerTile.gameObject.transform.position.y + ownerTile.getHeight() + 0.5f, ownerTile.gameObject.transform.position.z), Quaternion.identity,
      ownerTile.transform);
    ownerTile.additionalHeight += 1f;
    attachListener(caster,EventHook.endTurn);
  }

  protected override void onDeactivateEffects() {
    ownerTile.additionalHeight -= 1f;
    if (ownerTile.occupant) ownerTile.occupant.transform.position = new Vector3(ownerTile.occupant.transform.position.x, ownerTile.gameObject.transform.position.y + ownerTile.getHeight() + 0.5f, ownerTile.occupant.transform.position.z);
    Object.Destroy(block);
    detachListener(caster);
  }

  public override bool shouldDecrement(Event e) {
    return e.sender == caster;
  }
}

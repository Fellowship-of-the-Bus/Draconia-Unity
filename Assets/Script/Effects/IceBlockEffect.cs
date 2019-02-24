using UnityEngine;

public class IceBlockEffect : DurationEffect {
  static GameObject blockPrefab = (GameObject)Resources.Load("Map/Doodads/CubeIce");
  GameObject block;
  public BattleCharacter caster;
  protected override void onActivate() {
    block = (GameObject) GameObject.Instantiate(blockPrefab, new Vector3(ownerTile.transform.position.x, ownerTile.transform.position.y + ownerTile.getHeight() + 0.5f, ownerTile.transform.position.z), Quaternion.identity,
      ownerTile.transform);
    block.transform.localScale = new Vector3(block.transform.localScale.x/ownerTile.transform.localScale.x,
                                        block.transform.localScale.y/ownerTile.transform.localScale.y,
                                        block.transform.localScale.z/ownerTile.transform.localScale.z);
    ownerTile.additionalHeight += 1f;
    attachListener(caster,EventHook.endTurn);
  }

  protected override void onDeactivateEffects() {
    ownerTile.additionalHeight -= 1f;
    if (ownerTile.occupant) ownerTile.occupant.transform.position = new Vector3(ownerTile.occupant.transform.position.x, ownerTile.transform.position.y + ownerTile.getHeight() + 0.5f, ownerTile.occupant.transform.position.z);
    Object.Destroy(block);
    detachListener(caster);
  }

  public override bool shouldDecrement(Draconia.Event e) {
    return e.sender == caster;
  }
}

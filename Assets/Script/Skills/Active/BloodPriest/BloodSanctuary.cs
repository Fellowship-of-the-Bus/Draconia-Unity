using UnityEngine;
using System.Collections.Generic;

public class BloodSanctuary: ActiveSkill, AoeSkill {
  public int aoe {get; set;}
  public bool effectsTiles {get; set;}


  public BloodSanctuary() {
    range = 3;
    aoe = 0;
    useLos = false;
    name = "Blood Sanctuary";
    effectsTiles = true;
    maxCooldown = 1;
    targetsTiles = true;
  }

  public override List<GameObject> getTargets() {
    GameManager gm = GameManager.get;
    List<Tile> tiles = gm.map.getTilesWithinRange(self.curTile, range);
    List<GameObject> targets = new List<GameObject>();
    foreach (Tile t in tiles) {
      targets.Add(t.gameObject);
    }
    targets.Add(self.curTile.gameObject);
    return targets;
  }

  public List<GameObject> getTargetsInAoe(Vector3 position) {
    GameManager gm = GameManager.get;
    List<Tile> tiles = gm.map.getTilesWithinRange(gm.map.getTile(position), aoe);
    List<GameObject> targets = new List<GameObject>();
    foreach (Tile t in tiles) {
        targets.Add(t.gameObject);
    }
    targets.Add(gm.map.getTile(position).gameObject);
    return targets;
  }

  public override void tileEffects(Tile target) {
    self.takeDamage((int)(self.intelligence * 0.5));
    BloodSanctuaryEffect block = new BloodSanctuaryEffect();
    block.level = level;
    block.duration = 2;
    block.caster = self;
    target.applyEffect(block);
  }


}

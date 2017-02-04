using UnityEngine;
using System.Collections.Generic;

public class ScorchEarth: ActiveSkill, AoeSkill {
  public int aoe {get; set;}
  public bool effectsTiles {get; set;}

  public ScorchEarth() {
    range = 3;
    aoe = 0;
    useLos = false;
    name = "Scorch Earth";
    effectsTiles = true;
    maxCooldown = 1;
  }

  public override List<GameObject> getTargets() {
    GameManager gm = GameManager.get;
    List<Tile> tiles = gm.getTilesWithinRange(self.curTile, range);
    List<GameObject> targets = new List<GameObject>();
    foreach (Tile t in tiles) {
      targets.Add(t.gameObject);
    }
    targets.Add(self.curTile.gameObject);
    return targets;
  }

  public List<GameObject> getTargetsInAoe(Vector3 position) {
    GameManager gm = GameManager.get;
    List<Tile> tiles = gm.getTilesWithinRange(gm.getTile(position), aoe);
    List<GameObject> targets = new List<GameObject>();
    foreach (Tile t in tiles) {
        targets.Add(t.gameObject);
    }
    targets.Add(gm.getTile(position).gameObject);
    return targets;
  }

  public override void tileEffects(Tile target) {
    ScorchEarthEffect burn = new ScorchEarthEffect();
    burn.level = level;
    burn.duration = 2;
    burn.caster = self;
    target.applyEffect(burn);
  }

  public override int calculateDamage(Character source, Character target) {
    return (int)((source.intelligence*(1+level*0.1) - target.magicDefense) * target.fireResMultiplier);
  }


}

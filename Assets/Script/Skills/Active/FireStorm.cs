using UnityEngine;
using System.Collections.Generic;

public class FireStorm: ActiveSkill, AoeSkill {
  public int aoe {get; set;}
  public bool effectsTiles {get; set;}

  public FireStorm() {
    range = 3;
    useWepRange = false;
    aoe = 2;
    useLos = false;
    name = "FireStorm";
    effectsTiles = false;
    cooldown = 2;
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
        if (t.occupant) targets.Add(t.occupant);
        else targets.Add(t.gameObject);
    }
    return targets;
  }


  public override int calculateDamage(Character source, Character target) {
    return (int)((source.intelligence*(1+level*0.1) - target.magicDefense)*target.fireResMultiplier);
  }


}

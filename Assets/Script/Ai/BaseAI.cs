using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Linq;

public abstract class BaseAI {
  public BattleCharacter owner;
  public virtual void init() {}
  public abstract void target();
  public abstract Vector3 move();

  protected struct TargetSet {
    public Tile tile;
    public List<BattleCharacter> affected;
  }

  protected List<TargetSet> getTargetSets(ActiveSkill skill, List<Tile> targets, Tile userTile) {
    AoeSkill aoe = skill as AoeSkill;

    // Determine possible targets
    List<TargetSet> targetSets = new List<TargetSet>();
    if (aoe != null) {
      foreach(Tile t in targets) {
        TargetSet tSet;
        tSet.tile = t;

        List<Tile> affectedTiles = aoe.getTargetsInAoe(t.gameObject.transform.position);
        affectedTiles = new List<Tile>(affectedTiles.Filter((x) => x.occupied()));
        tSet.affected = new List<BattleCharacter>(affectedTiles.Select(x => x.occupant));
        if (affectedTiles.Contains(userTile)) {
          tSet.affected.Add(owner);
        } else {
          tSet.affected.Remove(owner);
        }

        targetSets.Add(tSet);
      }
    } else {
      List<BattleCharacter> chars = new List<BattleCharacter>(targets.Select(x => x.occupant));
      chars = new List<BattleCharacter>(chars.Filter((character) => character != null));
      foreach (BattleCharacter t in chars) {
        TargetSet tSet;
        tSet.tile = t.curTile;

        List<BattleCharacter> singleton = new List<BattleCharacter>();
        singleton.Add(t);
        tSet.affected = singleton;

        targetSets.Add(tSet);
      }
    }

    return targetSets;
  }
}

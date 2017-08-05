using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Linq;

public abstract class BaseAI {
  public BattleCharacter owner;
  public abstract void target();
  public abstract Vector3 move();

  protected List<List<BattleCharacter>> getTargetSets(ActiveSkill skill, List<Tile> targets) {
    AoeSkill aoe = skill as AoeSkill;

    // Determine possible targets
    List<List<BattleCharacter>> targetCharacters = new List<List<BattleCharacter>>();
    if (aoe != null) {
      foreach(Tile t in targets) {
        List<Tile> affectedTiles = aoe.getTargetsInAoe(t.gameObject.transform.position);
        affectedTiles = new List<Tile>(affectedTiles.Filter((x) => x.occupied()));
        targetCharacters.Add(new List<BattleCharacter>(affectedTiles.Select(x => x.occupant)));
      }
    } else {
      List<BattleCharacter> chars = new List<BattleCharacter>(targets.Select(x => x.occupant));
      chars = new List<BattleCharacter>(chars.Filter((character) => character != null && character.team != owner.team));
      foreach (BattleCharacter t in chars) {
        List<BattleCharacter> singleton = new List<BattleCharacter>();
        singleton.Add(t);
        targetCharacters.Add(singleton);
      }
    }

    return targetCharacters;
  }
}

using UnityEngine;
using System;
using System.Collections.Generic;

public class Sentry : PassiveSkill {
  public int aoe {get; set;}

  ActiveSkill skill = new ArcShot();
  public Sentry() {
    aoe = 3;
    useLos = false;
    skill.level = level;
  }

  protected override void additionalEffect(Draconia.Event e) {
    List<Tile> tiles = GameManager.get.map.getTilesWithinRange(owner.curTile, aoe, false);
    if (e.sender.team == owner.team) return; // don't shoot teammates
    if (tiles.Find(t => t.transform.position == e.position) != null) {
      List<Tile> target = new List<Tile>();
      target.Add(e.sender.curTile);
      owner.useSkill(skill, new List<Tile>(target));
    }
  }

  protected override void onActivate() {
    Debug.AssertFormat(owner != null, "Sentry activated before owner is set.");
    attachListener(EventManager.get, EventHook.enterTile);
    skill.self = owner;
  }

  protected override void onDeactivate() {
    detachListener(EventManager.get);
  }
}


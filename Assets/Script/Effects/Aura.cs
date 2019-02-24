using UnityEngine;
using System;
using System.Collections.Generic;

public class Aura<T> : DurationEffect where T: Effect, new() {
  public int radius;
  public bool applyToSelf = true;
  private bool applyToEnemies = false;
  private Func<T> effectFactory;
  private Dictionary<BattleCharacter, T> affected = new Dictionary<BattleCharacter, T>();

  public Aura(int r, Func<T> f, bool enemyAffecting = false) {
    radius = r;
    effectFactory = f;
    applyToEnemies = enemyAffecting;
  }

  protected override void additionalEffect(Draconia.Event e) {
    if (e.hook == EventHook.preMove) {
      removeAura();
    } else if (e.hook == EventHook.postMove) {
      addAura();
    }
  }

  public override void activate() {
    attachListener(GameManager.get.eventManager, EventHook.preMove);
    attachListener(GameManager.get.eventManager, EventHook.postMove);
    addAura();
  }

  protected override void onDeactivateEffects() {
    removeAura();
  }

  protected override void onDeactivateListeners() {
    detachListener(GameManager.get.eventManager);
  }

  private void addAura() {
    List<Tile> tiles = GameManager.get.map.getTilesWithinRange(owner.curTile, radius);

    if (applyToSelf) {
      setup(owner);
    }
    foreach (Tile t in tiles) {
      if (t.occupied()) {
        BattleCharacter c = t.occupant.GetComponent<BattleCharacter>();
        if (c.team == owner.team ^ applyToEnemies) {
          setup(c);
        }
      }
    }
  }

  private void removeAura() {
    foreach (KeyValuePair<BattleCharacter, T> c in affected) {
      c.Key.removeEffect(c.Value);
    }
    affected = new Dictionary<BattleCharacter, T>();
  }

  private void setup(BattleCharacter c) {
    T effect = effectFactory();
    effect.level = level;
    c.applyEffect(effect);
    affected.Add(c, effect);
  }
}

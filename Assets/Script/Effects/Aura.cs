using UnityEngine;
using System;
using System.Collections.Generic;

public class Aura<T> : DurationEffect where T: Effect, new() {
  public int radius;
  public bool applyToSelf = true;
  private Func<T> effectFactory;
  private Dictionary<Character, T> affected = new Dictionary<Character, T>();

  public Aura(int r, Func<T> f) {
    radius = r;
    effectFactory = f;
  }

  protected override void additionalEffect(Event e) {
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
    List<Tile> tiles = GameManager.get.getTilesWithinRange(owner.curTile, radius);

    if (applyToSelf) {
      setup(owner);
    }
    foreach (Tile t in tiles) {
      if (t.occupied()) {
        Character c = t.occupant.GetComponent<Character>();
        if (c.team == owner.team) {
          setup(c);
        }
      }
    }
  }

  private void removeAura() {
    foreach (KeyValuePair<Character, T> c in affected) {
      c.Key.removeEffect(c.Value);
    }
    affected = new Dictionary<Character, T>();
  }

  private void setup(Character c) {
    T effect = effectFactory();
    effect.level = level;
    c.applyEffect(effect);
    affected.Add(c, effect);
  }
}

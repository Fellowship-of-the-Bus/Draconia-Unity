﻿using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ForceShot: SingleTarget {

  public ForceShot() {
    requireWeapon(Weapon.kinds.Ranged);
    useWepRange = true;
    useLos = true;
    name = "Force Shot";
    maxCooldown = 2;
  }

  float upThreshold = 0.5f;

  Tile knockTo(Character c) {
    Vector3 heading = c.gameObject.transform.position - self.gameObject.transform.position;
    Vector3 direction = heading / heading.magnitude;
    direction.x = Mathf.Round(direction.x);
    direction.z = Mathf.Round(direction.z);

    Tile t = GameManager.get.map.getTile(c.gameObject.transform.position + direction);
    return t;
  }

  public override void additionalEffects(Character c) {
    Tile t = knockTo(c);
    GameManager game = GameManager.get;
    Map map = game.map;
    if (t != null && !t.occupied() && ((map.getHeight(t) + upThreshold) > map.getHeight(t))) {
      game.updateTile(c,t);
      LinkedList<Tile> tile = new LinkedList<Tile>();
      tile.AddFirst(t);
      game.moving = true;
      game.waitToEndTurn(game.StartCoroutine(game.IterateMove(tile, c.gameObject)));
    }
  }

  public override int damageFormula() {
    return (int)(self.strength*(1+level*0.1));
  }
}

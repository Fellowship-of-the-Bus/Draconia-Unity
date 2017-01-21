using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ForceShot: SingleTarget {

  public ForceShot() {
    useWepRange = true;
    useLos = true;
    name = "Force Shot";
    cooldown = 2;
  }

  float upThreshold = 0.5f;

  Tile knockTo(Character c) {
    Vector3 heading = c.gameObject.transform.position - self.gameObject.transform.position;
    Vector3 direction = heading / heading.magnitude;
    direction.x = Mathf.Round(direction.x);
    direction.z = Mathf.Round(direction.z);

    Tile t = GameManager.get.getTile(c.gameObject.transform.position + direction);
    return t;
  }

  public override void additionalEffects(Character c) {
    range = self.attr.weaponRange;

    Tile t = knockTo(c);
    if (t != null && !t.occupied() && ((GameManager.get.getHeight(t) + upThreshold) > GameManager.get.getHeight(t))) {
      GameManager.get.updateTile(c,t);
      LinkedList<Tile> tile = new LinkedList<Tile>();
      tile.AddFirst(t);
      GameManager.get.moving = true;
      GameManager.get.waitToEndTurn(GameManager.get.StartCoroutine(GameManager.get.IterateMove(tile, c.gameObject, false)));
    }
  }

  public override int calculateDamage(Character source, Character target) {
    return (int)(source.attr.strength*(1+level*0.1) - target.attr.physicalDefense);
  }
}

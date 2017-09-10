using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class HookShot: SingleTarget {

  public HookShot() {
    requireWeapon(Weapon.Kinds.Ranged);
    useWepRange = true;
    useLos = true;
    name = "Hook Shot";
    maxCooldown = 2;
    targetAlly(false);
    targetEnemy(true);
  }

  float upThreshold = 0.5f;

  Tile pullTo(BattleCharacter c) {
    Vector3 heading =  self.transform.position - c.transform.position;
    Vector3 direction = heading / heading.magnitude;
    direction.x = Mathf.Round(direction.x);
    direction.z = Mathf.Round(direction.z);

    Tile t = GameManager.get.map.getTile(c.transform.position + direction);
    return t;
  }

  public override void additionalEffects(BattleCharacter c) {
    Tile t = pullTo(c);
    GameManager game = GameManager.get;
    Map map = game.map;
    if (t != null && !t.occupied() && ((map.getHeight(t) + upThreshold) > map.getHeight(t))) {
      game.updateTile(c,t);
      LinkedList<Tile> tile = new LinkedList<Tile>();
      tile.AddFirst(t);
      game.moving = true;
      game.waitFor(game.StartCoroutine(game.IterateMove(tile, c.gameObject, game.getWaitingIndex())));
    }
  }

  public override int damageFormula() {
    return (int)(self.strength*(1+level*0.1));
  }
}

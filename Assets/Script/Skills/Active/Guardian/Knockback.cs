using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Knockback: TargetMover {
  public Knockback() {
    requireMelee();
    useWepRange = true;
    useLos = false;
    name = "Knockback";
    maxCooldown = 2;
    targetAlly(false);
    targetEnemy(true);
    setDirection(TargetMover.Direction.away);
  }

  public override string tooltip { get { return "Deal " + damageFormula().ToString() + " damage and knock the target back"; }}
  float upThreshold = 0.5f;

  bool validDestination(BattleCharacter c, Tile t) {
    return validIntermediate(c, t) && !t.occupied();
  }

  bool validIntermediate(BattleCharacter c, Tile t) {
    GameManager game = GameManager.get;
    Map map = game.map;
    return t != null && ((map.getHeight(c.curTile) + upThreshold) > map.getHeight(t));
  }

  Tile knockTo(BattleCharacter c) {
    Vector3 heading = c.transform.position - self.transform.position;
    Vector3 direction = heading / heading.magnitude;
    direction.x = Mathf.Round(direction.x);
    direction.z = Mathf.Round(direction.z);

    Tile t = GameManager.get.map.getTile(c.transform.position + direction);
    Tile t2 = GameManager.get.map.getTile(c.transform.position + (direction * 2));
    if (validIntermediate(c, t)) {
      if (validDestination(c, t2)) {
        return t2;
      } else if ( validDestination(c, t)) {
        return t;
      }
    }
    return null;
  }

  public override void additionalEffects(BattleCharacter c) {
    GameManager game = GameManager.get;

    Tile t = knockTo(c);
    if (validDestination(c, t) && c.isAlive()) {
      game.updateTile(c,t);
      LinkedList<Tile> tile = new LinkedList<Tile>();
      tile.AddFirst(t);
      game.moving = true;
      game.waitFor(game.StartCoroutine(game.IterateMove(tile, c.gameObject, game.getWaitingIndex(), false)));
    }
  }

  public override int damageFormula() {
    return (int)(self.strength*(1+level*0.1));
  }
}

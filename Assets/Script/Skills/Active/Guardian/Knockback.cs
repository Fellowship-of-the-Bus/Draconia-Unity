using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Knockback: SingleTarget {
  public Knockback() {
    requireMelee();
    useWepRange = true;
    useLos = false;
    name = "Knockback";
    maxCooldown = 2;
    targetAlly(false);
    targetEnemy(true);
  }

  public override string tooltip { get { return "Deal " + damageFormula().ToString() + " damage and knock the target back"; }}
  float upThreshold = 0.5f;

  Tile knockTo(BattleCharacter c) {
    Vector3 heading = c.transform.position - self.transform.position;
    Vector3 direction = heading / heading.magnitude;
    direction.x = Mathf.Round(direction.x);
    direction.z = Mathf.Round(direction.z);

    Tile t = GameManager.get.map.getTile(c.transform.position + direction);
    return t;
  }

  public override void additionalEffects(BattleCharacter c) {
    Tile t = knockTo(c);
    if (t != null && !t.occupied() && ((GameManager.get.map.getHeight(c.curTile) + upThreshold) > GameManager.get.map.getHeight(t))) {
      GameManager.get.movePiece(c, t);
    }
  }

  public override int damageFormula() {
    return (int)(self.strength*(1+level*0.1));
  }
}

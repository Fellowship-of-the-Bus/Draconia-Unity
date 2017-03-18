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
  }

  public override string tooltip { get { return "Deal " + damageFormula().ToString() + " damage and knock the target back"; }}
  float upThreshold = 0.5f;

  Tile knockTo(Character c) {
      Vector3 heading = c.gameObject.transform.position - self.gameObject.transform.position;
      Vector3 direction = heading / heading.magnitude;
      Tile t = GameManager.get.getTile(c.gameObject.transform.position + direction);
      return t;
  }

  public override void additionalEffects(Character c) {
    Tile t = knockTo(c);
    if (t != null && !t.occupied() && ((GameManager.get.getHeight(t) + upThreshold) > GameManager.get.getHeight(t))) {
      GameManager.get.MovePiece(c, t);
    }
  }

  public override int damageFormula() {
    return (int)(self.strength*(1+level*0.1));
  }
}

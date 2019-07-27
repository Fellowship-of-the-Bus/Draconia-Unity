using UnityEngine;
using System.Collections.Generic;

public class Fireball: SingleTarget {

  public override string animation { get { return "Cast"; }}
  public override Color castColor { get { return castColorPyromancer; }}

  public Fireball() {
    range = 3;
    useWepRange = false;
    useLos = true;
    name = "Fireball";
    maxCooldown = 2;
    intAligned();

    dType = DamageType.magical;
    dEle = DamageElement.fire;
    targetAlly(false);
    targetEnemy(true);
    projectileType = ProjectileType.Fireball;
    projectileMoveType = ProjectileMovementType.Straight;
    projectileSpeed = 1.5f;
  }

  protected override string tooltipDescription { get {
    return "Hurl a ball of fire that deals " + tooltipDamage + " damage to the target";
  }}

  public override int damageFormula(){
    return (int)(self.intelligence*(1.4+level*0.1));
  }
}

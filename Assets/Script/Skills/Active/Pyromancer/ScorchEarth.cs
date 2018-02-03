using UnityEngine;
using System.Collections.Generic;

public class ScorchEarth: ActiveSkill, AoeSkill {
  public int aoe {get; set;}
  public bool effectsTiles {get; set;}

  public override string animation { get { return "Cast"; }}

  public ScorchEarth() {
    range = 3;
    aoe = 0;
    useLos = false;
    name = "Scorch Earth";
    effectsTiles = true;
    maxCooldown = 1;
    targetsTiles = true;

    dType = DamageType.magical;
    dEle = DamageElement.fire;
    targetAlly(false);
    targetEnemy(true);
  }

  public List<Tile> getTargetsInAoe(Vector3 position) {
    return getTargetsInAoe(position, aoe);
  }

  public override void tileEffects(Tile target) {
    ScorchEarthEffect burn = new ScorchEarthEffect();
    burn.level = level;
    burn.duration = 3;
    burn.caster = self;
    target.applyEffect(burn);
  }

  public override int damageFormula() {
    return (int)(self.intelligence*(1+level*0.1));
  }
}

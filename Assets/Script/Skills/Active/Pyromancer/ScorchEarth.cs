using UnityEngine;
using System.Collections.Generic;

public class ScorchEarth: ActiveSkill, AoeSkill {
  public int aoe {get; set;}
  public bool effectsTiles {get; set;}


  public ScorchEarth() {
    range = 3;
    aoe = 0;
    useLos = false;
    name = "Scorch Earth";
    effectsTiles = true;
    maxCooldown = 1;
    targetsTiles = true;
  }

  public override List<GameObject> getTargets() {
    return tileTargetting();
  }

  public List<GameObject> getTargetsInAoe(Vector3 position) {
    return getTargetsInAoe(position, aoe);
  }

  public override void tileEffects(Tile target) {
    ScorchEarthEffect burn = new ScorchEarthEffect();
    burn.level = level;
    burn.duration = 2;
    burn.caster = self;
    target.applyEffect(burn);
  }

  public override int calculateDamage(Character source, Character target) {
    return (int)((source.intelligence*(1+level*0.1) - target.magicDefense) * target.fireResMultiplier);
  }


}

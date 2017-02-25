using UnityEngine;
using System.Collections.Generic;

public class IceBlock: ActiveSkill, AoeSkill {
  public int aoe {get; set;}
  public bool effectsTiles {get; set;}

  public IceBlock() {
    range = 3;
    aoe = 0;
    useLos = false;
    name = "Ice Block";
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
    IceBlockEffect block = new IceBlockEffect();
    block.level = level;
    block.duration = 2;
    block.caster = self;
    target.applyEffect(block);
  }

  public override int calculateDamage(Character source, Character target) {
    return (int)((source.intelligence*(1+level*0.1) - target.magicDefense) * target.iceResMultiplier);
  }
}

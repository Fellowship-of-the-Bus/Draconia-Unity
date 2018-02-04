using UnityEngine;
using System.Collections.Generic;

public class IceBlock: ActiveSkill, AoeSkill {
  public int aoe {get; set;}
  public bool effectsTiles {get; set;}

  public override string animation { get { return "Cast"; }}

  public IceBlock() {
    range = 3;
    aoe = 0;
    useLos = false;
    name = "Ice Block";
    effectsTiles = true;
    maxCooldown = 1;
    targetsTiles = true;

    dType = DamageType.magical;
    dEle = DamageElement.ice;
    targetAlly(true);
    targetEnemy(true);
  }

  public List<Tile> getTargetsInAoe(Vector3 position) {
    return getTargetsInAoe(position, aoe);
  }

  public override void tileEffects(Tile target) {
    IceBlockEffect block = new IceBlockEffect();
    block.level = level;
    block.duration = 2;
    block.caster = self;
    target.applyEffect(block);
  }

  public override int damageFormula() {
    return (int)(self.intelligence*(1+level*0.1));
  }
}

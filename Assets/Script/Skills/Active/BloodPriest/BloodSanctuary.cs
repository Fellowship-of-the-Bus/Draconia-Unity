using UnityEngine;
using System.Collections.Generic;

public class BloodSanctuary: ActiveSkill, AoeSkill {
  public int aoe {get; set;}
  public bool effectsTiles {get; set;}

  public override string animation { get { return "Cast"; }}
  public override Color castColor { get { return castColorBloodPriest; }}

  public BloodSanctuary() {
    range = 3;
    aoe = 0;
    useLos = false;
    name = "Blood Sanctuary";
    effectsTiles = true;
    maxCooldown = 1;
    targetsTiles = true;
    targetAlly(true);
    targetEnemy(true);
  }

  public List<Tile> getTargetsInAoe(Vector3 position) {
    return getTargetsInAoe(position, aoe);
  }

  public override void tileEffects(Tile target) {
    self.takeDamage((int)(self.intelligence * 0.5),self);
    BloodSanctuaryEffect block = new BloodSanctuaryEffect();
    block.effectValue = level;
    block.duration = 2;
    block.caster = self;
    target.applyEffect(block);
  }
}

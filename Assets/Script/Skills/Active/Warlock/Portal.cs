using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Portal : ActiveSkill, AoeSkill {
  public int aoe {get; set;}
  public bool effectsTiles {get; set;}

  public override string animation { get { return "Cast"; }}
  public override Color castColor { get { return castColorWarlock; }}

  public Portal() {
    ntargets = 2;
    range = 5;
    aoe = 0;
    useLos = false;
    name = "Portal";
    effectsTiles = true;
    maxCooldown = 1;
    targetsTiles = true;
    targetAlly(true);
    targetEnemy(true);
  }

  public override BattleCharacter self {
    set {
      base.self = value;
      attachListener(self, EventHook.postSkill);
    }
  }

  public List<Tile> getTargetsInAoe(Vector3 position) {
    return getTargetsInAoe(position, aoe);
  }

  protected override void trigger(Event e) {
    if (e.skillUsed != this) return;
    List<Effected> targets = e.targets;
    Debug.AssertFormat(targets != null, "Portal on event with null targets");
    Debug.AssertFormat(targets.Count == 2, "Portal on event with wrong number of targets {0}", targets.Count);
    PortalEffect eff1 = makeEffect();
    PortalEffect eff2 = makeEffect();
    eff1.sibling = eff2;
    eff2.sibling = eff1;
    eff1.effected = eff2.effected = new List<BattleCharacter>();
    targets[0].applyEffect(eff1);
    targets[1].applyEffect(eff2);
  }

  PortalEffect makeEffect() {
    PortalEffect block = new PortalEffect();
    block.level = level;
    block.duration = 10;
    block.caster = self;
    return block;
  }

  public override void validate(List<Tile> targets) {
    // prevent targetting the same tile twice
    if (targets.Count == 2) {
      if (targets.First() == targets.Last()) {
        targets.RemoveAt(1);
      }
    }
  }
}

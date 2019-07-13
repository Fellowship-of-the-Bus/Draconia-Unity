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

  protected override string tooltipDescription { get {
    return "Conjure a portal between two targeted positions allowing characters to move between them";
  }}

  public override BattleCharacter self {
    set {
      base.self = value;
      attachListener(self, EventHook.postSkill);
    }
  }

  public List<Tile> getTargetsInAoe(Vector3 position) {
    return getTargetsInAoe(position, aoe);
  }

  protected override void trigger(Draconia.Event e) {
    if (e.skillUsed != this) return;
    List<Effected> targets = e.targets;
    Debug.AssertFormat(targets != null, "Portal on Draconia.Event with null targets");
    Debug.AssertFormat(targets.Count == 2, "Portal on Draconia.Event with wrong number of targets {0}", targets.Count);
    PortalEffect eff1 = makeEffect();
    PortalEffect eff2 = makeEffect();
    eff1.sibling = eff2;
    eff2.sibling = eff1;
    eff1.effected = eff2.effected = new List<BattleCharacter>();
    eff1.caster = self;
    targets[0].applyEffect(eff1);
    eff2.caster = self;
    targets[1].applyEffect(eff2);
  }

  PortalEffect makeEffect() {
    PortalEffect block = new PortalEffect();
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

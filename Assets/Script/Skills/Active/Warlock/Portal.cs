using System;
using System.Collections.Generic;
using UnityEngine;

public class Portal : ActiveSkill, AoeSkill {
  public int aoe {get; set;}
  public bool effectsTiles {get; set;}

  public Portal() {
    ntargets = 2;
    range = 5;
    aoe = 0;
    useLos = false;
    name = "Portal";
    effectsTiles = true;
    maxCooldown = 1;
    targetsTiles = true;
  }

  public override Character self { 
    set {       
      base.self = value;
      attachListener(self, EventHook.postSkill);
    }
  }

  public override List<GameObject> getTargets() {
    return tileTargetting();
  }

  public List<GameObject> getTargetsInAoe(Vector3 position) {
    return getTargetsInAoe(position, aoe);
  }

  protected override void trigger(Event e) {
    List<Effected> targets = e.targets;
    Debug.AssertFormat(targets != null, "Portal on event with null targets");
    Debug.AssertFormat(targets.Count == 2, "Portal on event with wrong number of targets {0}", targets.Count);
    PortalEffect eff1 = makeEffect();
    PortalEffect eff2 = makeEffect();
    eff1.sibling = eff2;
    eff2.sibling = eff1;
    eff1.effected = eff2.effected = new List<Character>();
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

  public override void validate(List<List<Effected>> targets) {
    // prevent targetting the same tile twice
    if (targets.Count == 2) {
      Debug.Log("two targets");
      if (targets[0][0] == targets[1][0]) {
        Debug.Log("Targets are the same");
        targets.RemoveAt(1);
      }
    }
  }
}

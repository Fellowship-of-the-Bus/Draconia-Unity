using UnityEngine;
using System.Collections.Generic;
using System;

public class Reciprocate : PassiveSkill {
  protected override void onActivate() {
    attachListener(owner, EventHook.useBuffSkill);
  }
  protected override void onDeactivate() {
    detachListener(owner);
  }
  protected override void additionalEffect(Draconia.Event e) {
    Effect buff = e.appliedBuff.clone();
    buff.caster = self;
    owner.applyEffect(buff);
  }
}

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
  protected override void additionalEffect(Event e) {
    Effect buff = e.appliedBuff.clone();
    owner.applyEffect(buff);
  }
}

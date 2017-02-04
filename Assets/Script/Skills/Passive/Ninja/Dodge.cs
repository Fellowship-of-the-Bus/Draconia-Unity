﻿using System;
using UnityEngine;
using System.Collections.Generic;

public class Dodge : PassiveSkill {
  protected override void onActivate() {
    attachListener(owner, EventHook.preDamage);
  }
  protected override void onDeactivate() {
    detachListener(owner);
  }

  protected override void additionalEffect(Event e) {
    float chance = UnityEngine.Random.value;
    if (chance < 0.1*level) {
      e.finishAttack = false;
//      Debug.Log("dodged: " + e.finishAttack);
    } else {
//      Debug.Log("did not dodge: " + e.finishAttack);
    }
  }
}

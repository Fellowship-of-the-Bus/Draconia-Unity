using UnityEngine;
using System.Collections.Generic;
using System;

public class Adrenaline : PassiveSkill {
  public int maxCooldown = 4;
  int timeToActivate = 0;

  public Adrenaline() {
    name = "Adrenaline";
  }

  protected override string tooltipDescription { get {
    string tooltipCooldown = "";
    if (timeToActivate != 0) {
      tooltipCooldown = "\nCooldown remaining: " + timeToActivate.ToString();
    }
    return "Remain at 1 health after receiving lethal damage. Only triggers once every " +
      maxCooldown.ToString() + " turns." + tooltipCooldown;
  }}

  protected override void onActivate() {
    attachListener(owner, EventHook.preDeath);
    attachListener(owner, EventHook.endTurn);
  }
  protected override void onDeactivate() {
    detachListener(owner);
  }
  protected override void additionalEffect(Draconia.Event e) {
    if (e.hook == EventHook.preDeath && timeToActivate == 0) {
      e.preventDeath = true;
      timeToActivate = maxCooldown;
    } else if (e.hook == EventHook.endTurn) {
      timeToActivate = (int)Math.Max(timeToActivate - 1, 0);
    }
  }
}

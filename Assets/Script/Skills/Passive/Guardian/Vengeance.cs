using System;
using UnityEngine;
using System.Collections.Generic;

public class Vengeance : PassiveSkill {
  public Vengeance() {
    name = "Vengeance";
  }

  protected override string tooltipDescription { get {
    return "Taking damage increases strength";
  }}

  protected override void onActivate() {
    self.attrChange.strength += strBonus((float)self.curHealth / self.maxHealth);
    attachListener(owner, EventHook.postDamage);
  }
  protected override void onDeactivate() {
    detachListener(owner);
  }

  private int strBonus(float percent) {
    return (int)Math.Round(3 * level * (1 - percent));
  }

  protected override void additionalEffect(Draconia.Event e) {
    int originalHealth = self.curHealth + e.damageTaken;
    self.attrChange.strength += strBonus((float)self.curHealth / self.maxHealth)
                              - strBonus((float)originalHealth / self.maxHealth);
  }
}

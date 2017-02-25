using UnityEngine;
using System.Collections.Generic;

public class ShareLife: CircleAoE, HealingSkill {
  public ShareLife() {
    range = 3;
    useWepRange = false;
    aoe = 2;
    useLos = false;
    name = "Share Life";
    effectsTiles = false;
    maxCooldown = 2;
  }

  public override Character self { 
    set {       
      base.self = value;
      attachListener(self, EventHook.postSkill);
    }
  }

  protected override void trigger(Event e) {
    if (e.hook == EventHook.postSkill) {
      if ((int)(self.intelligence*(1+level*0.1)) >= self.curHealth) {
        if (self.curHealth > 1) self.takeDamage(self.curHealth - 1);
      }
      else self.takeDamage((int)(self.intelligence*(1+level*0.1)));
    }
  }

  public int calculateHealing(Character source, Character target) {
    int amount = (int)(source.intelligence*(1+level*0.1));
    if (amount >= source.curHealth) amount = source.curHealth - 1;
    return amount;
  }

  public override List<GameObject> getTargetsInAoe(Vector3 position) {
    List<GameObject> l = base.getTargetsInAoe(position);
    l.Remove(self.gameObject);
    List<GameObject> l2 = new List<GameObject>(l.Filter((go) => go.GetComponent<Character>() == null || go.GetComponent<Character>().team == self.team));
    return l2;
  }
}

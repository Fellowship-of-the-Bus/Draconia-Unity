using System;
using UnityEngine;
using System.Collections.Generic;

public class IronSkin : PassiveSkill {
  public override List<GameObject> getTargets() {
    List<GameObject> targets = new List<GameObject>();
    targets.Add(self.gameObject);
    return targets;
  }

  int numHits;
  int addedDefense = 0;
  bool inTurn = false;

  public override void onActivate() {
    attachListener(owner, EventHook.endTurn);
    attachListener(owner, EventHook.postDamage);
    attachListener(owner, EventHook.startTurn);
  }
  public override void onDeactivate() {
    detachListener(owner);
  }

  private int defBonus() {
    return (int)(level * numHits * .2f);
  }

  public override void additionalEffect(Event e) {
    switch (e.hook) {
      case EventHook.startTurn:
        self.attr.physicalDefense -= addedDefense;
        addedDefense = 0;
        numHits = 0;
        inTurn = true;
        break;
      case EventHook.endTurn:
        inTurn = false;
        break;
      case EventHook.postDamage:
        if (!inTurn) {
          numHits++;
          addedDefense += defBonus();
          owner.attr.physicalDefense += defBonus();
        }
        break;
    }
  }
}

using System;
using UnityEngine;
using System.Collections.Generic;

public class IronSkin : PassiveSkill {
  int numHits;
  int addedDefense = 0;
  bool inTurn = false;

  protected override void onActivate() {
    attachListener(owner, EventHook.endTurn);
    attachListener(owner, EventHook.postDamage);
    attachListener(owner, EventHook.startTurn);
  }
  protected override void onDeactivate() {
    detachListener(owner);
  }

  private int defBonus() {
    return (int)(level * numHits * .2f);
  }

  protected override void additionalEffect(Draconia.Event e) {
    switch (e.hook) {
      case EventHook.startTurn:
        self.attrChange.physicalDefense -= addedDefense;
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
          owner.attrChange.physicalDefense += defBonus();
        }
        break;
    }
  }
}

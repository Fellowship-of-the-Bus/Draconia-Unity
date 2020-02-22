using System;
using UnityEngine;
using System.Collections.Generic;

public class IronSkin : PassiveSkill {
  int numHits = 0;
  int addedDefense = 0;
  bool inTurn = false;

  public IronSkin() {
    name = "Iron Skin";
  }

  public override string tooltipDescription { get {
    string bonusTooltip = "";
    if (addedDefense != 0) {
      bonusTooltip = "\n\nHits Taken: " + numHits.ToString()
        + "\nDefense Increase: " + addedDefense.ToString();
    }
    return "Increases physical defense by each time damage is taken." + bonusTooltip;
  }}

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

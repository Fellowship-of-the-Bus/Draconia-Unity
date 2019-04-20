using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;

public class BrodricDies : Objective {
  public BrodricDies() {
    description = "Brodric must survive.";
  }
  override public bool isMet(GameManager game) {
    List<BattleCharacter> allies = game.players;
    if (allies.Count == 0) return true;
    foreach(BattleCharacter c in allies) {
      if (c.name == "Brodric") {
        return false;
      }
    }
    return true;
  }
}

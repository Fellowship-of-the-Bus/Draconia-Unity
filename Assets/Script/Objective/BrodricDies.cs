using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;

public class BrodricDies : Objective {
  override public bool isMet(GameManager game) {
    List<GameObject> allies = game.players;
    if (allies.Count == 0) return true;
    foreach(GameObject o in allies) {
      if (o.GetComponent<BattleCharacter>().name == "Brodric") {
        return false;
      }
    }
    return true;
  }
}

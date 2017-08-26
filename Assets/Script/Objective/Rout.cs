using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;

public class Rout : Objective {
  public Rout() {
    description = "Defeat all Enemy units.";
  }
  override public bool isMet(GameManager game) {
    List<GameObject> enemies = game.enemies;
    if (enemies.Count == 0) return true;
    else return false;
  }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;

public class Rout : Objective {
  override public bool isMet(GameManager game) {
    GameObject[] enemies = GameObject.FindGameObjectsWithTag("PiecePlayer2");
    if (enemies.Length == 0) return true;
    else return false;
  }
}

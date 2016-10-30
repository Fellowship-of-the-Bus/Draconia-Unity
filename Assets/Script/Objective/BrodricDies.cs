using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;

public class BrodricDies : Objective {
  override public bool isMet(GameManager game) {
    GameObject[] allies = GameObject.FindGameObjectsWithTag("PiecePlayer1");
    //TODO: change brodric detection from speed == 10
    if (allies.Length == 0) return true;
    foreach(GameObject o in allies) {
      if (o.GetComponent<Character>().attr.speed == 10) {
        return false;
      }
    }
    return true;
  }
}

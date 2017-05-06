using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class DamageAll : BFTimeEvent{

  public override void trigger() {
    foreach (GameObject c in GameManager.get.players) {
      c.GetComponent<Character>().takeDamage(2);
    }
    foreach (GameObject c in GameManager.get.enemies) {
      c.GetComponent<Character>().takeDamage(3);
    }
  }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;

public class BasicMoveAI : BaseMoveAI {
  Character owner;
  public override Vector3 move() {
    GameManager game = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    return new Vector3(0,0,0);

  }
}

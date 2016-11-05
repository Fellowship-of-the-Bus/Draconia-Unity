using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;

public abstract class BaseMoveAI {
  public Character owner;
  public abstract Vector3 move();
}

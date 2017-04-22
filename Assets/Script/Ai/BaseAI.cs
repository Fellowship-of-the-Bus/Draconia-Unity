using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;

public abstract class BaseAI {
  public Character owner;
  public abstract void target();
  public abstract Vector3 move();
}

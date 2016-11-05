using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;

public abstract class BaseAttackAI {
  public Character owner;
  public abstract void target();
}

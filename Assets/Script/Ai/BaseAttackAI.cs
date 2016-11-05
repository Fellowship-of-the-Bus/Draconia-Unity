using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;

public abstract class BaseAttackAI {
  public Character owner;
  public abstract int getAttackRange();
  public abstract void target(List<GameObject> targets);
}

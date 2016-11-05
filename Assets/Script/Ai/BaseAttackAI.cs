using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;

public abstract class BaseAttackAI {
  Character owner;
  public abstract Vector3 target(List<GameObject> targets);
}

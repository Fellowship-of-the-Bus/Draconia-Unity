using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;

public abstract class Objective {
  public abstract bool isMet(GameManager game);
}

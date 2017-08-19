using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;

public class ObjectiveFactory {
  //identifier is the name of the subclass extending Objective
  public static Objective makeObjective(string identifier) {
    Objective o = null;
    switch(identifier) {
      case "BrodricDies":
        o = new BrodricDies();
        break;
      case "Rout":
        o = new Rout();
        break;
      default:
        Debug.AssertFormat(false, "Unknown Objective name: " + identifier);
        break;
    }
    return o;
  }
}

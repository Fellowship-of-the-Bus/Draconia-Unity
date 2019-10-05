using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;

// Deprecated
public class SkillCharSelect: InvCharSelect {

  public SkillSelectController controller;

  protected override void onButtonClick(Selection s){
    base.onButtonClick(s);
    controller.setChar(s.c);
  }

}

﻿using UnityEngine;

public class CrippleEffect : Effect {
  public override void onRemove() {

  }
  public override void onActivate() {
    owner.moveRange -= level;
  }
  public override void onDeactivate() {
    owner.moveRange += level;
  }
  public override void onEvent(Event e) {

  }
}

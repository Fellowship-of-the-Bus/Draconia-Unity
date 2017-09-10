using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class TimeDialogue : BFTimeEvent{

  List<DialogueFragment> d;
  public TimeDialogue(int t, List<DialogueFragment> dialogues) : base(t) {
    d = dialogues;
  }

  public override void trigger() {
    GameManager.get.dialogue.loadDialogue(d);
  }
}
